USE [Metis]
GO
/****** Object:  View [dbo].[view_resource]    Script Date: 12/14/2018 12:46:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[view_resource] AS
SELECT 
	 Resource_id 
	,Resource_name
	,Supervisor_id
	,Supervisor_name,
	email_v_resource email
	,supervisor_email
	,is_admin
	,is_interviewer
	,rfs_consultant_id
	,hrms_attendance_id
	,hrms_candidate_id
	,hrms_employee_id
	,JOINING_DATE
	,CAREER_START_DATE
	,IS_METIS_ADMIN
	,DESIGNATIONNAME
	,hrms_interviewer_id
	,candidategender gender 
	FROM 
	[STREEBORECRUITINGSYSTEM].[streeboRecruitingSystem].dbo.v_Individual where status=1
	--[10.0.100.24].[StreeboEDW].[dbo].[view_resource]

GO
