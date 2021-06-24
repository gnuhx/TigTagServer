Create Procedure [dbo].[GetAllJobsPaging](
   @countSkip int,
   @countTake int,
   @computer VARCHAR(100),
   @printer VARCHAR(100),
   @title VARCHAR(100),
   @status VARCHAR(100)
)
as
begin
DECLARE @query VARCHAR(max) = '
;with paging as (
     Select 
		ROW_NUMBER() OVER (ORDER BY j.Id desc) as RowCounts,
		j.Id as JobId,
		j.Title as Title,
		j.FileUrl as FileUrl,
		p.ComputerId as ComputerId,
		c.Name as ComputerName,
		p.Name as PrinterName,
		j.PrinterId as PrinterId,
		j.CreatedOnUtc as CreatedOn,
		Case j.[Status] 
			when 0 then ''Message pending send queue''
			when 1 then ''Message sent queue success''
			when 2 then ''Client received message''
			when 3 then ''Client print completed''
			when 4 then ''Error''
			else null
		END AS [Status]
	 from dbo.Job as j 
	 join dbo.Printer as p on j.PrinterId= p.Id
	 join dbo.Computer as c on c.Id=p.ComputerId
	 where j.Id = j.Id '

	 + CASE WHEN NULLIF(@title, '') IS NOT NULL THEN 
	 + ' and j.Title like ''%'+@title+'%'' '
	 ELSE '' END 

	 + CASE WHEN NULLIF(@computer, '') IS NOT NULL THEN 
	 + ' and p.ComputerId in (select * from dbo.nop_splitstring_to_table(''' + convert(nvarchar(20), @computer) + ''', '','')) '
	 ELSE '' END 

	 + CASE WHEN NULLIF(@printer, '') IS NOT NULL THEN 
	 + ' and j.PrinterId in (select * from dbo.nop_splitstring_to_table(''' + convert(nvarchar(20), @printer) + ''', '','')) '
	 ELSE '' END 

	 + CASE WHEN NULLIF(@status, '') IS NOT NULL THEN 
	 + ' and j.[Status] in (select * from dbo.nop_splitstring_to_table(''' + convert(nvarchar(20), @status) + ''', '','')) '
	 ELSE '' END 

	 + ' )'
	 + 'select max(p.RowCounts) RowCounts,
			0 JobId,
			null Title,
			null FileUrl,
			null ComputerId,
			null ComputerName,
			null PrinterName,
			null PrinterId,
			null CreatedOn,
			null [Status]
		from paging p

		union all

		select * from paging p
		where p.RowCounts > ' + CONVERT(nvarchar(10), @countSkip) + ' and p.RowCounts <= ' + CONVERT(nvarchar(10), (@countSkip + @countTake)) + ''

	 PRINT (@query)
	 EXECUTE  (@query)
end
