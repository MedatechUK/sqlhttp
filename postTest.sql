declare @response xml
declare @hdoc int

BEGIN TRY
	select @response = dbo.postxml('http://localhost/api/demo/hours.ashx', dbo.xmlCustomers())	
	EXEC sp_xml_preparedocument @hdoc OUTPUT, @response

	select	
		[status],
		[message]
	FROM OPENXML(@hdoc, 'response',8)
	with (
		[status] int,
		[message] varchar(max),
		[stacktr] varchar(max)
	)

	EXEC sp_xml_removedocument @hdoc

END TRY

BEGIN CATCH
	SELECT 500, ERROR_MESSAGE()

END CATCH

