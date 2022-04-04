
IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE name = 'sp_SurveyHeader'
		)
	DROP PROC dbo.sp_SurveyHeader
GO

CREATE PROC dbo.sp_SurveyHeader (
	@accion CHAR(1) = NULL,
	@i_name  VARCHAR(50) = NULL,
	@i_description VARCHAR(100) = NULL,
	@i_surveyId  VARCHAR(50) = NULL
	)
AS
--------------------Grabar---------------------------------
IF (@accion = 'I')
BEGIN

	INSERT INTO [dbo].[SurveyHeader]
			   ([SurveyId]
			   ,[Name]
			   ,[Description]
			   ,[status])
		 VALUES
			   (@i_surveyId,
			   @i_name,
			   @i_description,
			   1
			   )
	SELECT * from [dbo].[SurveyHeader] WHERE [SurveyId]=@i_surveyId
END

IF (@accion = 'C')
BEGIN
SELECT [SurveyId]
      ,[Name]
      ,[Description]
      ,[status]
  FROM [dbo].[SurveyHeader] WHERE [SurveyId]=@i_surveyId
END

IF (@accion = 'L')
BEGIN

	SELECT [SurveyId]
      ,[Name]
      ,[Description]
      ,[status]
	FROM [dbo].[SurveyHeader]
END

IF (@accion = 'M')
BEGIN

	INSERT INTO [dbo].[SurveyHeader]
			   ([SurveyId]
			   ,[Name]
			   ,[Description]
			   ,[status])
		 VALUES
			   (@i_surveyId,
			   @i_name,
			   @i_description,
			   1
			   )
	SELECT * from [dbo].[SurveyHeader] WHERE [SurveyId]=@i_surveyId
END

IF (@accion = 'D')
BEGIN
DELETE FROM [dbo].[SurveyDetail]
      WHERE SurveyId=@i_surveyId
DELETE FROM [dbo].[SurveyHeader]
      WHERE SurveyId=@i_surveyId

END
