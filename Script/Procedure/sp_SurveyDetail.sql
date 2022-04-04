
IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE name = 'sp_SurveyDetail'
		)
	DROP PROC dbo.sp_SurveyDetail
GO

CREATE PROC dbo.sp_SurveyDetail (
	@accion CHAR(1) = NULL,
	@xml_detalle xml=NULL,
	@i_surveyId VARCHAR(50) = NULL
	)
AS
--------------------Grabar---------------------------------
IF (@accion = 'I')
BEGIN

	INSERT INTO [dbo].[SurveyDetail]
           ([SurveyId]
           ,[FieldName]
           ,[FieldType]
           ,[FieldTitle]
           ,[Required]
           ,[Status],
			[ItemId])
     
	SELECT 
		T.C.value('(surveyid/text())[1]', 'VARCHAR(50)') as SurveyId,
		T.C.value('(fieldname/text())[1]', 'VARCHAR(50)') as FieldName,
		T.C.value('(fieldtype/text())[1]', 'VARCHAR(20)') as FieldType,
		T.C.value('(fieldtitle/text())[1]', 'VARCHAR(50)') as FieldTitle,
		T.C.value('(required/text())[1]', 'bit') as Required,
		T.C.value('(status/text())[1]', 'bit') as Status,
		T.C.value('(itemid/text())[1]', 'VARCHAR(50)') as ItemId
	
	from @xml_detalle.nodes('/datos/detalle') T(C)
END

IF (@accion = 'D')
BEGIN

	DELETE FROM [dbo].[SurveyDetail]
      WHERE SurveyId=@i_surveyId
END

IF (@accion = 'L')
BEGIN
SELECT [ItemId]
      ,[SurveyId]
      ,[FieldName]
      ,[FieldType]
      ,[FieldTitle]
      ,[Required]
      ,[Status]
  FROM [dbo].[SurveyDetail] WHERE [SurveyId]=@i_surveyId
END