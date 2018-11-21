declare @response xml
declare @hdoc int

BEGIN TRY
	select @response = dbo.getxml('http://localhost/api/demo/customers.ashx')	
	EXEC sp_xml_preparedocument @hdoc OUTPUT, @response

	select [CUSTNAME] , [CUSTDES] , [NAME]
	FROM OPENXML( @hdoc, 'customers/customer/contacts',2)
	with (
		[CUSTNAME] varchar(50) '../CUSTNAME',
		[CUSTDES] varchar(50) '../CUSTDES',
		[NAME] varchar(50)
	)

	EXEC sp_xml_removedocument @hdoc

END TRY

BEGIN CATCH
	SELECT 500, ERROR_MESSAGE()

END CATCH

