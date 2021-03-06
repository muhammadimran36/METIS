USE [Metis]
GO
/****** Object:  StoredProcedure [dbo].[getAllBulkAssignments]    Script Date: 01/22/2013 13:51:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[getAllBulkAssignments]
AS
BEGIN
      select w.id AS BulkAssignmentID,
      Replace(Convert(varchar,w.Week_endings,113),' ','-') WeekEnding,
      r.Resource_name ResourceName,
      p.Project_name ProjectName,
      Bulk_Ass BulkAssignment,
      Work_load BulkWorkLoad,
      Replace(Convert(varchar,start_bulk,113),' ','-') BulkStartDate,
      Replace(Convert(varchar,end_bulk,113),' ','-') BulkEndDate
      from  Weekly_Reports w
      left outer join [view_project] P on w.Project_id = p.Project_id
      left outer join [view_resource] R on w.Resource_id=r.Resource_id
      where w.Bulk_Ass is not null and w.Bulk_Ass<>''
      and (w.isDeleted is null or w.isDeleted <>'true')
      ORDER BY 3,4;
END
      