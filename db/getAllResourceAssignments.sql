USE [Metis]
GO
/****** Object:  StoredProcedure [dbo].[getAllResourceAssignments]    Script Date: 01/22/2013 13:06:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[getAllResourceAssignments] 
@user varchar(255)
AS
BEGIN
SET NOCOUNT ON;
WITH Subordinates AS
(
    SELECT vrs.resource_id,vrs.Resource_name
    FROM v_Resource AS vrs
    WHERE  vrs.resource_id = (SELECT vr.resource_id FROM v_Resource vr WHERE vr.email  = @user)
    
    UNION ALL
    
    SELECT vrs.resource_id,vrs.Resource_name
    FROM v_Resource AS vrs
    WHERE vrs.Supervisor_id = (SELECT vr.resource_id FROM v_Resource vr WHERE vr.email = @user)
   
    UNION ALL

    SELECT vrs.resource_id,vrs.Resource_name
    FROM v_Resource AS vrs
    INNER JOIN Subordinates AS sub ON vrs.Supervisor_id = sub.resource_id
)
select 
	a.A_id ResourceAssignmentID,
	r.Resource_id ResourceID,
	r.Resource_name ResourceName,
	p.Project_id ProjectID,
	p.Project_name ProjectName,
	rs.Role_id RoleID,
	rs.Role_title RoleName
	from Assignment a,
	view_resource r,
	view_project p,
	Roles rs,
	Subordinates sb
	where a.Resource_id = r.Resource_id
	and a.Project_id = p.Project_id
	and a.Role_id=rs.Role_id
	and sb.resource_id COLLATE SQL_Latin1_General_CP1_CI_AS = r.Resource_id
	
	
END





