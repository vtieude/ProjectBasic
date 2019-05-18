Project information
--------------------BackEnd------------------------------
- Asp.Net core API 2.2
* Have two way approach will use: 
- Use database first: to update and generate model from existing database
Scaffold-DbContext "Server=localhost;Database=BaseDatabase;User Id=sa;Password=123456;Trusted_Connection=True;Integrated Security=false;"
Microsoft.EntityFrameworkCore.SqlServer -OutputDir DAL/Models -Force -Context FirstContext
- Use code first: Define and create the model on project
+Run the ‘Enable-Migrations’ command in Package Manager Console
+Add-Migration will scaffold the next migration based on changes you have made to your model.
+Update-Database will apply any pending changes to the database.
* Structure project
- Project Core: Contains all model can re-use for all project in solotion.
- The modal and dbcontext contain on DAL/Models
- Remember always add all property for all model to tracking the change on database :  
	public DateTimeOffset? SysChgDate { get; set; }
        public string SysChgLogin { get; set; }
        public string SysChgType { get; set; }
        public int? SysChgCnt { get; set; }