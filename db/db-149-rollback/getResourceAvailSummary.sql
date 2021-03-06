USE [Metis]
GO
/****** Object:  StoredProcedure [dbo].[getResourceAvailSummary]    Script Date: 01/22/2013 13:52:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[getResourceAvailSummary]
@WeekStarting Date,
@WeekEnding Date
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
Set @qryWeeklyMaster1 = 'select r.Resource_id,r.Resource_name,d.Dept_name';
Set @qryWeeklyMaster2 = ' from [view_resource] r left outer join Weekly_Reports w on r.Resource_id=w.Resource_id 
   left outer join  Resource_association ra on r.Resource_id=ra.Resource_id left outer join Department d on d.Dept_id = ra.Dept_id  group by r.Resource_id,r.Resource_name,d.Dept_name  
   order by r.Resource_name ';

While @counter <= @diff

Begin
     Set @counter = @counter + 1;
     --Set @qryWeeklyMaster1 += ',ISNULL((select 40-(Sum(monday)+SUM(tuesday)+Sum(wednesday)+SUM(thursday)+Sum(friday)+SUM(saturday)+SUM(sunday)) as weekSum from [view_resource] s left outer join Weekly_Reports w on r.Resource_id=w.Resource_id and (w.isDeleted is null or LOWER(w.isDeleted)<>''true'')  and Convert(varchar,w.Week_endings,120) like '''+Convert(varchar(max),@WeekEnding,120)+''' where s.Resource_id = r.Resource_id),40) ['+Convert(varchar(max),@WeekStarting,113)+']';
     Set @qryWeeklyMaster1 += ',ISNULL((select 40-(ISNULL((SELECT SUM(monday+tuesday+wednesday+thursday+friday+saturday+sunday) 
       From [view_resource] s 
	   INNER Join Weekly_Reports w on r.Resource_id=w.Resource_id 
	   WHERE (w.isDeleted is null or LOWER(w.isDeleted)<>''true'') 
	   AND Convert(varchar,w.Week_endings,120) = '''+Convert(varchar(max),@WeekEnding,120)+'''AND s.Resource_id = r.Resource_id),0)
	   +
	   (ISNULL((select Sum(lw.COUNT) FROM view_resource s INNER JOIN LeavesWeekWise lw ON s.Resource_id = lw.Resourceid AND s.Resource_id = r.Resource_id AND Convert(varchar,lw.Saturday_Of_Week,120) = '''+Convert(varchar(max),@WeekEnding,120)+'''),0)))),40) ['+Convert(varchar(max),@WeekStarting,113)+']';
     Set @WeekEnding = DATEADD(DAY,  7, @WeekEnding);
     Set @WeekStarting = DATEADD(DAY,  7, @WeekStarting);
End
     
     Set @qryWeeklyMaster1 += @qryWeeklyMaster2;
     EXECUTE sp_executeSQL @qryWeeklyMaster1;
END
