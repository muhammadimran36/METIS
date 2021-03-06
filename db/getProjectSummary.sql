USE [Metis]
GO
/****** Object:  StoredProcedure [dbo].[getProjectSummary]    Script Date: 01/24/2013 15:14:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[getProjectSummary]
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
--Setting Variables 
Set @counter = 0;
/*
Set @qryWeeklyMaster1 = 
'SELECT p.Project_id,p.Project_name Project,d.Dept_name';
Set @qryWeeklyMaster2 = ' FROM [view_project] p   
LEFT OUTER JOIN Weekly_Reports w ON w.Project_id=p.Project_id 
LEFT OUTER JOIN [view_resource] r  on r.Resource_id=w.Resource_id 
LEFT OUTER JOIN  Resource_association ra on r.Resource_id=ra.Resource_id 
LEFT OUTER JOIN Department d ON d.Dept_id = ra.Dept_id 
GROUP BY p.Project_id,p.Project_name,d.Dept_name';
While @counter < 16

Begin
     Set @counter = @counter + 1;
     Set @qryWeeklyMaster1 += ',(select Sum(monday)+SUM(tuesday)+Sum(wednesday)+SUM(thursday)+Sum(friday)+SUM(saturday)+SUM(sunday) as weekSum from [view_project] s left outer join Weekly_Reports w on s.Project_id=w.Project_id and (w.isDeleted is null or LOWER(w.isDeleted)<>''true'') and Convert(varchar,w.Week_endings,120) like '''+Convert(varchar(max),@WeekEnding,120)+''' where s.Project_id = p.Project_id) ['+Convert(varchar(max),@WeekStarting,113)+']';
     Set @WeekEnding = DATEADD(DAY,  7, @WeekEnding);
     Set @WeekStarting = DATEADD(DAY,  7, @WeekStarting);
End
*/
Set @qryWeeklyMaster1 = 
'WITH Subordinates AS
(
    SELECT vrs.resource_id,vrs.Resource_name
    FROM v_Resource AS vrs
    WHERE  vrs.resource_id = (SELECT vr.resource_id FROM v_Resource vr WHERE vr.email  ='''+@user+''')
    
    UNION ALL
    
    SELECT vrs.resource_id,vrs.Resource_name
    FROM v_Resource AS vrs
    WHERE vrs.Supervisor_id = (SELECT vr.resource_id FROM v_Resource vr WHERE vr.email ='''+@user+''')
   
    UNION ALL

    SELECT vrs.resource_id,vrs.Resource_name
    FROM v_Resource AS vrs
    INNER JOIN Subordinates AS sub ON vrs.Supervisor_id = sub.resource_id
 )
SELECT p.Project_id,p.Project_name Project';
Set @qryWeeklyMaster2 = ' FROM [view_project] p   
INNER JOIN Weekly_Reports w ON p.Project_id = w.Project_id
INNER JOIN Subordinates s ON s.resource_id  COLLATE SQL_Latin1_General_CP1_CI_AS = w.resource_id
WHERE w.IsDeleted <> ''true''
GROUP BY p.Project_id,p.Project_name';
While @counter < 16

Begin
     Set @counter = @counter + 1;
     Set @qryWeeklyMaster1 += ',(select Sum(monday)+SUM(tuesday)+Sum(wednesday)+SUM(thursday)+Sum(friday)+SUM(saturday)+SUM(sunday) as weekSum from [view_project] s left outer join Weekly_Reports w on s.Project_id=w.Project_id and (w.isDeleted is null or LOWER(w.isDeleted)<>''true'') and Convert(varchar,w.Week_endings,120) like '''+Convert(varchar(max),@WeekEnding,120)+''' where s.Project_id = p.Project_id) ['+Convert(varchar(max),@WeekStarting,113)+']';
     Set @WeekEnding = DATEADD(DAY,  7, @WeekEnding);
     Set @WeekStarting = DATEADD(DAY,  7, @WeekStarting);
End

     Set @qryWeeklyMaster1 += @qryWeeklyMaster2;
     EXECUTE sp_executeSQL @qryWeeklyMaster1;
END