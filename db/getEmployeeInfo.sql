USE [Metis]
GO
/****** Object:  StoredProcedure [dbo].[getEmployeeInfo]    Script Date: 01/28/2013 04:44:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[getEmployeeInfo]
@id nvarchar(10)
AS
BEGIN
DECLARE @yy INT
DECLARE @mm INT
DECLARE @getmm INT
DECLARE @careerStartDate date
DECLARE @joiningDate date
DECLARE @totalITexp varchar(50);
DECLARE @streeboExp varchar(50);

SELECT  @careerStartDate =  CAREER_START_DATE 
FROM 
    [view_resource]
	WHERE resource_id = @id

SELECT  @joiningDate =  JOINING_DATE 
FROM 
 [view_resource]
WHERE resource_id = @id


--Calculation Total IT Exp
SET @yy = (DATEDIFF(DAY, @careerStartDate, GETDATE()))/365
SET @mm = DATEDIFF(mm,@careerStartDate, GETDATE())
SET @getmm = ABS(DATEDIFF(mm, DATEADD(yy,@yy,@careerStartDate),GETDATE()))
IF(@yy>0)
BEGIN
	SET @totalITexp = Convert(varchar(10),@yy) + ' year ' 
END
ELSE
BEGIN
SET @totalITexp = ''
END
IF(@getmm>0)
BEGIN
	SET @totalITexp += Convert(varchar(10),@getmm) + ' month'
END

--Calculation Streebo Exp
SET @yy = (DATEDIFF(DAY,@joiningDate,GETDATE()))/365
SET @mm = DATEDIFF(mm,@joiningDate,GETDATE())
SET @getmm = ABS(DATEDIFF(mm, DATEADD(yy,@yy,@joiningDate),GETDATE()))
IF(@yy>0)
BEGIN
SET @streeboExp = Convert(varchar(10),@yy) + ' year(s) ' 
END
ELSE
BEGIN
SET @streeboExp = ''
END
IF(@getmm>0)
BEGIN
	SET @streeboExp += Convert(varchar(10),@getmm) + ' month(s)'
END

SELECT
    Resource_name
   ,CASE [DESIGNATIONNAME] WHEN NULL THEN 'N/A' WHEN '' THEN 'N/A' ELSE [DESIGNATIONNAME] END AS [DESIGNATIONNAME]
    --,ISNULL([DESIGNATIONNAME],'N/A') [DESIGNATIONNAME] 
    ,CASE @totalITexp WHEN NULL THEN 'N/A' WHEN '' THEN 'N/A' ELSE @totalITexp END AS [TotalITExp]
    ,CASE @streeboExp WHEN NULL THEN 'N/A' WHEN '' THEN 'N/A' ELSE @streeboExp END AS [TotalStreeboExp]
	FROM 
	[view_resource]
	WHERE resource_id = @id
END