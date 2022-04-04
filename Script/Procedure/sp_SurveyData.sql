
IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE name = 'sp_SurveyData'
		)
	DROP PROC dbo.sp_SurveyData
GO

CREATE PROC dbo.sp_SurveyData (
	@accion CHAR(1) = NULL,
	@xml_detalle xml=NULL,
	@i_surveyId VARCHAR(59)=null
	)
AS
--------------------Grabar---------------------------------
IF (@accion = 'I')
BEGIN

	INSERT INTO [dbo].[SurveyData]
           ([SurveyId]
           ,[ItemId]
           ,[Information]
           ,[DataId]
           ,[FieldName])
     
	SELECT 
		T.C.value('(surveyid/text())[1]', 'VARCHAR(50)') as SurveyId,
		T.C.value('(itemid/text())[1]', 'VARCHAR(50)') as ItemId,
		T.C.value('(information/text())[1]', 'VARCHAR(MAX)') as Information,
		T.C.value('(dataid/text())[1]', 'VARCHAR(50)') as DataId,
		T.C.value('(fieldname/text())[1]', 'VARCHAR(50)') as FieldName
		
	from @xml_detalle.nodes('/datos/detalle') T(C)
END

IF (@accion = 'D')
BEGIN

	DELETE FROM [dbo].[SurveyData]
      WHERE SurveyId=@i_surveyId
END

IF (@accion = 'L')
BEGIN
SELECT [SurveyId]
      ,[ItemId]
      ,[Information]
      ,[DataId]
      ,[FieldName]
  FROM [dbo].[SurveyData]
END