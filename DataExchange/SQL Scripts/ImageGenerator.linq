<Query Kind="Program" />

void Main()
{
	StringBuilder sb = new StringBuilder();

	for(int i = 0; i < 50; i++)
	{ 
	
		sb.AppendLine("INSERT INTO [dbo].[Images]  ([Title], [Description], [Path], [CreationDate], [ModifiedOn], [AlbumId])");
		sb.AppendLine("VALUES");
		sb.AppendLine(" ('Test Image"+i+"', 'Test Image"+i+" Description', '/Uploads/5fa61093-c207-4bbf-b022-d62f443455fe.jpg', GetDate(), GetDate(), 1)");
		sb.AppendLine("GO");
		sb.AppendLine();
	}
	
	sb.ToString().Dump();
}
