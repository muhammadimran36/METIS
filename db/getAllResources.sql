USE [Metis]
GO
/****** Object:  StoredProcedure [dbo].[getAllResources]    Script Date: 01/21/2013 20:02:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[getAllResources]
@user varchar(255)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
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
select DISTINCT

r.Resource_id ResourceID,
r.Resource_name ResourceName
FROM view_resource r

  INNER JOIN Subordinates sb
   ON sb.resource_id COLLATE SQL_Latin1_General_CP1_CI_AS = r.Resource_id

--,(SELECT DISTINCT Resource_id FROM Assignment) a 
--WHERE r.Resource_id = a.Resource_id
ORDER BY 2;
END
