USE [Metis]
GO
/****** Object:  StoredProcedure [dbo].[getAllResources]    Script Date: 01/22/2013 13:52:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[getAllResources]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
SELECT 
r.Resource_id ResourceID,
r.Resource_name ResourceName
FROM view_resource r
--,(SELECT DISTINCT Resource_id FROM Assignment) a 
--WHERE r.Resource_id = a.Resource_id
ORDER BY 2;
END
