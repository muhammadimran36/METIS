USE [Metis]
GO
/****** Object:  StoredProcedure [dbo].[getResourceDetail]    Script Date: 12/14/2018 12:47:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  PROCEDURE [dbo].[getResourceDetail]
@rid int,
@WeekStarting Date,
@WeekEnding Date

AS
BEGIN
SET NOCOUNT ON;

--Declare @rid int = 170;
--Declare @WeekStarting Date = '2014-12-12';
--Declare @WeekEnding Date = '2014-12-26';
	
--Declaration of Variables 
Declare @counter int
Declare @qryWeeklyMaster1 nvarchar(max);
Declare @qryWeeklyMaster2 nvarchar(max);
Declare @diff int;
--Setting Variables 
Set @counter = 0;
Set @diff = DATEDIFF(WEEK,@WeekStarting,@WeekEnding);
-- Sarfaraz @26Dec18
-- commenting the weekend reseting against issue# MTS-172
--Reset WeekEnding
--Set @WeekEnding = DATEADD(DW,@diff,@WeekStarting);
Set @qryWeeklyMaster1 = 'select r.Resource_id,r.Resource_name,p.Project_id,p.Project_name Project,ro.Role_Title';
Set @qryWeeklyMaster2 = ' from [view_resource] r     
LEFT OUTER JOIN Weekly_Reports w on r.Resource_id COLLATE SQL_Latin1_General_CP1_CI_AS=w.Resource_id     
LEFT JOIN [view_project] p on w.Project_id=p.Project_id COLLATE SQL_Latin1_General_CP1_CI_AS  
LEFT OUTER JOIN Assignment a ON a.Resource_id = w.Resource_id
LEFT OUTER JOIN Roles ro ON a.Role_id = ro.Role_id
where r.Resource_id COLLATE SQL_Latin1_General_CP1_CI_AS='''+Convert(nvarchar(10),@rid)+''' 
AND a.Project_id = w.Project_id
GROUP BY r.Resource_id,r.Resource_name,p.Project_name,p.Project_id,ro.Role_Title ';

While @counter <= @diff

Begin
     
     
     Set @counter = @counter + 1;
     Set @qryWeeklyMaster1 += ',(select Sum(monday)+SUM(tuesday)+Sum(wednesday)+SUM(thursday)+Sum(friday)+SUM(saturday)+SUM(sunday) as weekSum from [view_resource] s left outer join Weekly_Reports w on s.Resource_id COLLATE SQL_Latin1_General_CP1_CI_AS=w.Resource_id and (w.isDeleted is null or LOWER(w.isDeleted)<>''true'') and Convert(varchar,w.Week_endings,120) like '''+Convert(varchar(max),@WeekEnding,120)+''' where s.Resource_id = r.Resource_id  and w.Project_id=p.Project_id COLLATE SQL_Latin1_General_CP1_CI_AS) ['+Convert(varchar(max),@WeekStarting,113)+']';
     Set @WeekEnding = DATEADD(DAY,  7, @WeekEnding);
     Set @WeekStarting = DATEADD(DAY,  7, @WeekStarting);
End
     Set @qryWeeklyMaster1 += @qryWeeklyMaster2;
     
     EXECUTE sp_executeSQL @qryWeeklyMaster1;
     
END

GO
