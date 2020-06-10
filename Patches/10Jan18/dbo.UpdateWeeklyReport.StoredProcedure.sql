USE [Metis]
GO
/****** Object:  StoredProcedure [dbo].[UpdateWeeklyReport]    Script Date: 12/14/2018 12:47:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateWeeklyReport]
@resourceID nvarchar(50),
@projectID nvarchar(50),
@weekEnding date,
@hourPerDay float,
@ReturnMessage varchar(255) out
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	-- if assignment hours updated then run update otherwise insert new record.
	if exists(select * from Weekly_Reports where  Resource_id=@resourceID AND Project_id=@projectID And isDeleted <> 'true'
	AND Convert(varchar(max),Week_endings,120)= Convert(varchar(max),@weekEnding,120))
	Begin
		if (@hourPerDay <=0)
		Begin
			UPDATE Weekly_Reports SET monday=@hourPerDay,tuesday=@hourPerDay,wednesday=@hourPerDay,thursday=@hourPerDay,friday=@hourPerDay, [isDeleted]='true'
			WHERE Resource_id=@resourceID AND Project_id=@projectID
			AND Convert(varchar(max),Week_endings,120)= Convert(varchar(max),@weekEnding,120)	
		End
		Else 
		Begin
			UPDATE Weekly_Reports SET monday=@hourPerDay,tuesday=@hourPerDay,wednesday=@hourPerDay,thursday=@hourPerDay,friday=@hourPerDay
			WHERE Resource_id=@resourceID AND Project_id=@projectID And isDeleted <> 'true'
			AND Convert(varchar(max),Week_endings,120)= Convert(varchar(max),@weekEnding,120)	
		End
    End
	Else
	Begin
		INSERT INTO [Weekly_Reports]
			   ([Resource_id],[Project_id],[Work_days],[Available_days],[Week_endings]
			   ,[sunday],[monday],[tuesday],[wednesday],[thursday],[friday],[saturday]
			   ,[Bulk_Ass],[Work_load],[start_bulk],[end_bulk],[isDeleted],[AssignmentTypeID])
		 VALUES
			   (@ResourceID,@ProjectID,'','',@WeekEnding,
				0,@hourPerDay,@hourPerDay,@hourPerDay,@hourPerDay,@hourPerDay,0,
				'',0,'','','','B');
	End
    select @ReturnMessage = '';
	
END

GO
