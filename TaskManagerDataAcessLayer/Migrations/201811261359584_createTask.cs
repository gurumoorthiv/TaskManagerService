namespace TaskManagerDataAcessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createTask : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        Task_Id = c.Int(nullable: false, identity: true),
                        ParentTaskId = c.Int(),
                        TaskDescription = c.String(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        Priority = c.Int(nullable: false),
                        IsClosed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Task_Id)
                .ForeignKey("dbo.Tasks", t => t.ParentTaskId)
                .Index(t => t.ParentTaskId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "ParentTaskId", "dbo.Tasks");
            DropIndex("dbo.Tasks", new[] { "ParentTaskId" });
            DropTable("dbo.Tasks");
        }
    }
}
