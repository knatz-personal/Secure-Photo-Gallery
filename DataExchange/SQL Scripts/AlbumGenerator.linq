<Query Kind="Program" />

void Main()
{
	StringBuilder sb = new StringBuilder();

	for(int i = 0; i < 50; i++)
	{ 
	
		sb.AppendLine("INSERT INTO [dbo].[Albums]  ([Title], [Description], [CreationDate], [ModifiedOn], [UserId])");
		sb.AppendLine("VALUES");
		sb.AppendLine(" ('Test Album"+i+"', 'Test Album"+i+" Description', GetDate(), GetDate(), '642e74be-174b-47e1-a04c-2afcdc0be63d')");
		sb.AppendLine("GO");
		sb.AppendLine();
	}
	
	sb.ToString().Dump();
}