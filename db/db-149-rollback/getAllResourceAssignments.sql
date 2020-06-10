USE [Metis]
GO
/****** Object:  StoredProcedure [dbo].[getAllResourceAssignments]    Script Date: 01/22/2013 13:50:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[getAllResourceAssignments] 
AS
BEGIN
SET NOCOUNT ON;
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
	Roles rs
	where a.Resource_id = r.Resource_id
	and a.Project_id = p.Project_id
	and a.Role_id=rs.Role_id
	
END






