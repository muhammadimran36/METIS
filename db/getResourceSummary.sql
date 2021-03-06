USE [Metis]
GO
/****** Object:  StoredProcedure [dbo].[getResourceSummary]    Script Date: 01/21/2013 12:00:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[getResourceSummary]
@WeekStarting Date,
@WeekEnding Date,
@user varchar(255)
AS
BEGIN
SET NOCOUNT ON;

--Declaration of Variables 
Declare @counter int
Declare @qryWeeklyMaster1 nvarchar(max);
Declare @qryWeeklyMaster2 nvarchar(max);
Declare @diff int;

--Setting Variables 
Set @counter = 0;
Set @diff = DATEDIFF(WEEK,@WeekStarting,@WeekEnding);
--Reset WeekEnding
Set @WeekEnding = DATEADD(DW,5,@WeekStarting);

Set @qryWeeklyMaster1 = 
'WITH Subordinates AS
(
    SELECT vrs.resource_id,vrs.Resource_name
    FROM v_Resource AS vrs
    WHERE  vrs.resource_id = (SELECT vr.resource_id FROM v_Resource vr WHERE vr.email ='''+@user+''')
    
    UNION ALL
    
    SELECT vrs.resource_id,vrs.Resource_name
    FROM v_Resource AS vrs
    WHERE vrs.Supervisor_id = (SELECT vr.resource_id FROM v_Resource vr WHERE vr.email ='''+@user+''')
   
    UNION ALL

    SELECT vrs.resource_id,vrs.Resource_name
    FROM v_Resource AS vrs
    INNER JOIN Subordinates AS sub ON vrs.Supervisor_id = sub.resource_id
)
SELECT DISTINCT r.Resource_id,r.Resource_name,d.Dept_name';
Set @qryWeeklyMaster2 = ' FROM [view_resource] r LEFT OUTER JOIN Weekly_Reports w on r.Resource_id=w.Resource_id 
   LEFT OUTER JOIN  Resource_association ra on r.Resource_id=ra.Resource_id 
   LEFT OUTER JOIN Department d ON d.Dept_id = ra.Dept_id  
   INNER JOIN Subordinates sb
   ON sb.resource_id COLLATE SQL_Latin1_General_CP1_CI_AS = w.Resource_id
   group by r.Resource_id,r.Resource_name,d.Dept_name  
   order by r.Resource_name ';

While @counter <= @diff

Begin
       Set @counter = @counter + 1;
       Set @qryWeeklyMaster1 += ',Convert(varchar(max),
       ISNULL((SELECT SUM(monday+tuesday+wednesday+thursday+friday+saturday+sunday) 
       FROM [view_resource] s 
	   INNER Join Weekly_Reports w on r.Resource_id=w.Resource_id 
	   WHERE (w.isDeleted is null or LOWER(w.isDeleted)<>''true'') 
	   AND Convert(varchar,w.Week_endings,120) = '''+Convert(varchar(max),@WeekEnding,120)+'''AND s.Resource_id = r.Resource_id),0)
	   +
	   (ISNULL((SELECT Sum(lw.COUNT) FROM view_resource s INNER JOIN LeavesWeekWise lw ON s.Resource_id = lw.Resourceid AND s.Resource_id = r.Resource_id AND Convert(varchar,lw.Saturday_Of_Week,120) = '''+Convert(varchar(max),@WeekEnding,120)+'''),0))
	   )';
	  --Add Separator
     Set @qryWeeklyMaster1 += '+'' ''+''(''';
     Set @qryWeeklyMaster1 += '+Convert(varchar(max),ISNULL((SELECT Sum(lw.COUNT) FROM view_resource s INNER JOIN LeavesWeekWise lw ON s.Resource_id = lw.Resourceid AND s.Resource_id = r.Resource_id AND Convert(varchar,lw.Saturday_Of_Week,120) = '''+Convert(varchar(max),@WeekEnding,120)+'''),0))';
     Set @qryWeeklyMaster1 += '+'')'' '+'['+Convert(varchar(max),@WeekStarting,113)+']';
     Set @WeekEnding = DATEADD(DAY,  7, @WeekEnding);
     Set @WeekStarting = DATEADD(DAY,  7, @WeekStarting);
End
     Set @qryWeeklyMaster1 += @qryWeeklyMaster2;
     EXECUTE sp_executeSQL @qryWeeklyMaster1;
END
