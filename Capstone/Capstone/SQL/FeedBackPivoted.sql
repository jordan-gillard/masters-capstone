DECLARE @cols AS NVARCHAR(MAX),
    @query  AS NVARCHAR(MAX)

select @cols = STUFF((SELECT ',' + QUOTENAME(Question) 
                    from [dbo].[SurveySummary]
                    group by Question
                    order by Question
            FOR XML PATH(''), TYPE
            ).value('.', 'NVARCHAR(MAX)') 
        ,1,1,'')
set @query = N'SELECT CreatedOn, ' + @cols + N' from 
             (
                select CreatedOn, Question, Response from [dbo].[SurveySummary] where HospitalID = {0}
            ) x
            pivot 
            (
                max(Response)
                for Question in (' + @cols + N')
            ) p '
exec sp_executesql @query;