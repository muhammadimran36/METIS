USE [Metis]
GO
/****** Object:  StoredProcedure [dbo].[getAllBulkAssignments]    Script Date: 01/21/2013 20:51:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[getAllBulkAssignments]
@user varchar(255)
AS
BEGIN
WITH Subordinates AS
(
--    SELECT vrs.resource_id,vrs.Resource_name
--    FROM v_Resource AS vrs
--    WHERE  vrs.resource_id = (SELECT vr.resource_id FROM v_Resource vr WHERE vr.email  = @user)
    
--    UNION ALL
    
    SELECT vrs.resource_id,vrs.Resource_name
    FROM v_Resource AS vrs
    WHERE vrs.Supervisor_id = (SELECT vr.resource_id FROM v_Resource vr WHERE vr.email = @user)
   
    UNION ALL

    SELECT vrs.resource_id,vrs.Resource_name
    FROM v_Resource AS vrs
    INNER JOIN Subordinates AS sub ON vrs.Supervisor_id = sub.resource_id
)
      select DISTINCT w.id AS BulkAssignmentID,
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
      INNER JOIN Subordinates sb
      ON sb.resource_id COLLATE SQL_Latin1_General_CP1_CI_AS = r.Resource_id
      where w.Bulk_Ass is not null and w.Bulk_Ass<>''
      and (w.isDeleted is null or w.isDeleted <>'true')
      ORDER BY 3,4;
END
      