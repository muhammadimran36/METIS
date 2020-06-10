USE [Metis]
GO
/****** Object:  View [dbo].[view_resource2]    Script Date: 12/14/2018 12:46:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[view_resource2]
as  
--select * FROM [10.0.100.24].[StreeboEDW].[dbo].[view_resource2]
select
RESOURCE_ID,Resource_name,Supervisor_id,Supervisor_name,
email_v_resource email,supervisor_email,IS_ADMIN,IS_INTERVIEWER,RFS_CONSULTANT_ID,HRMS_ATTENDANCE_ID,HRMS_CANDIDATE_ID,HRMS_EMPLOYEE_ID,JOINING_DATE,CAREER_START_DATE,IS_METIS_ADMIN,HRMS_INTERVIEWER_ID,
candidategender gender, 'C' PUBLICATION_STATUS
FROM 
	[STREEBORECRUITINGSYSTEM].[streeboRecruitingSystem].dbo.v_Individual where status=1

GO
