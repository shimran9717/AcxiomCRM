using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditInfrastructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_AspNetUsers_AssignedTo",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Customers_CustomerId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Leads_LeadId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_AspNetUsers_OwnerId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_FollowUps_AspNetUsers_AssignedTo",
                table: "FollowUps");

            migrationBuilder.DropForeignKey(
                name: "FK_FollowUps_Customers_CustomerId",
                table: "FollowUps");

            migrationBuilder.DropForeignKey(
                name: "FK_FollowUps_Leads_LeadId",
                table: "FollowUps");

            migrationBuilder.DropForeignKey(
                name: "FK_Leads_AspNetUsers_AssignedTo",
                table: "Leads");

            migrationBuilder.DropForeignKey(
                name: "FK_Opportunities_AspNetUsers_AssignedTo",
                table: "Opportunities");

            migrationBuilder.DropForeignKey(
                name: "FK_Opportunities_Customers_CustomerId",
                table: "Opportunities");

            migrationBuilder.DropForeignKey(
                name: "FK_Opportunities_Leads_LeadId",
                table: "Opportunities");

            migrationBuilder.DropIndex(
                name: "IX_Opportunities_AssignedTo",
                table: "Opportunities");

            migrationBuilder.DropIndex(
                name: "IX_Opportunities_LeadId",
                table: "Opportunities");

            migrationBuilder.DropIndex(
                name: "IX_Leads_AssignedTo",
                table: "Leads");

            migrationBuilder.DropIndex(
                name: "IX_Leads_LeadCode",
                table: "Leads");

            migrationBuilder.DropIndex(
                name: "IX_FollowUps_AssignedTo",
                table: "FollowUps");

            migrationBuilder.DropIndex(
                name: "IX_Customers_CustomerCode",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_Email",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_OwnerId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_Phone",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Activities_AssignedTo",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "LeadId",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "LeadCode",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CustomerCode",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "Subject",
                table: "Activities");

            migrationBuilder.RenameColumn(
                name: "ExpectedValue",
                table: "Leads",
                newName: "EstimatedValue");

            migrationBuilder.RenameColumn(
                name: "Remarks",
                table: "FollowUps",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "FollowUpDate",
                table: "FollowUps",
                newName: "ScheduledDate");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "Customers",
                newName: "ApplicationUserId");

            migrationBuilder.RenameColumn(
                name: "OldValue",
                table: "AuditLogs",
                newName: "OldValues");

            migrationBuilder.RenameColumn(
                name: "NewValue",
                table: "AuditLogs",
                newName: "NewValues");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "AuditLogs",
                newName: "Timestamp");

            migrationBuilder.RenameColumn(
                name: "AssignedTo",
                table: "Activities",
                newName: "PerformedBy");

            migrationBuilder.AlterColumn<string>(
                name: "OpportunityName",
                table: "Opportunities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "Opportunities",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Opportunities",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Leads",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "LeadName",
                table: "Leads",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Leads",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Leads",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Subject",
                table: "FollowUps",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "FollowUps",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "FollowUps",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "OpportunityId",
                table: "FollowUps",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Customers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Customers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerName",
                table: "Customers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "CompanyName",
                table: "Customers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AuditLogs",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IpAddress",
                table: "AuditLogs",
                type: "character varying(45)",
                maxLength: 45,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Activities",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Activities",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OpportunityId",
                table: "Activities",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Opportunities_ApplicationUserId",
                table: "Opportunities",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_ApplicationUserId",
                table: "Leads",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowUps_ApplicationUserId",
                table: "FollowUps",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowUps_OpportunityId",
                table: "FollowUps",
                column: "OpportunityId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_ApplicationUserId",
                table: "Customers",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_EntityName",
                table: "AuditLogs",
                column: "EntityName");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_Timestamp",
                table: "AuditLogs",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_UserId",
                table: "AuditLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ApplicationUserId",
                table: "Activities",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_OpportunityId",
                table: "Activities",
                column: "OpportunityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_AspNetUsers_ApplicationUserId",
                table: "Activities",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Customers_CustomerId",
                table: "Activities",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Leads_LeadId",
                table: "Activities",
                column: "LeadId",
                principalTable: "Leads",
                principalColumn: "LeadId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Opportunities_OpportunityId",
                table: "Activities",
                column: "OpportunityId",
                principalTable: "Opportunities",
                principalColumn: "OpportunityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_AspNetUsers_ApplicationUserId",
                table: "Customers",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FollowUps_AspNetUsers_ApplicationUserId",
                table: "FollowUps",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FollowUps_Customers_CustomerId",
                table: "FollowUps",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_FollowUps_Leads_LeadId",
                table: "FollowUps",
                column: "LeadId",
                principalTable: "Leads",
                principalColumn: "LeadId");

            migrationBuilder.AddForeignKey(
                name: "FK_FollowUps_Opportunities_OpportunityId",
                table: "FollowUps",
                column: "OpportunityId",
                principalTable: "Opportunities",
                principalColumn: "OpportunityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_AspNetUsers_ApplicationUserId",
                table: "Leads",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Opportunities_AspNetUsers_ApplicationUserId",
                table: "Opportunities",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Opportunities_Customers_CustomerId",
                table: "Opportunities",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_AspNetUsers_ApplicationUserId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Customers_CustomerId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Leads_LeadId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Opportunities_OpportunityId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_AspNetUsers_ApplicationUserId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_FollowUps_AspNetUsers_ApplicationUserId",
                table: "FollowUps");

            migrationBuilder.DropForeignKey(
                name: "FK_FollowUps_Customers_CustomerId",
                table: "FollowUps");

            migrationBuilder.DropForeignKey(
                name: "FK_FollowUps_Leads_LeadId",
                table: "FollowUps");

            migrationBuilder.DropForeignKey(
                name: "FK_FollowUps_Opportunities_OpportunityId",
                table: "FollowUps");

            migrationBuilder.DropForeignKey(
                name: "FK_Leads_AspNetUsers_ApplicationUserId",
                table: "Leads");

            migrationBuilder.DropForeignKey(
                name: "FK_Opportunities_AspNetUsers_ApplicationUserId",
                table: "Opportunities");

            migrationBuilder.DropForeignKey(
                name: "FK_Opportunities_Customers_CustomerId",
                table: "Opportunities");

            migrationBuilder.DropIndex(
                name: "IX_Opportunities_ApplicationUserId",
                table: "Opportunities");

            migrationBuilder.DropIndex(
                name: "IX_Leads_ApplicationUserId",
                table: "Leads");

            migrationBuilder.DropIndex(
                name: "IX_FollowUps_ApplicationUserId",
                table: "FollowUps");

            migrationBuilder.DropIndex(
                name: "IX_FollowUps_OpportunityId",
                table: "FollowUps");

            migrationBuilder.DropIndex(
                name: "IX_Customers_ApplicationUserId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_AuditLogs_EntityName",
                table: "AuditLogs");

            migrationBuilder.DropIndex(
                name: "IX_AuditLogs_Timestamp",
                table: "AuditLogs");

            migrationBuilder.DropIndex(
                name: "IX_AuditLogs_UserId",
                table: "AuditLogs");

            migrationBuilder.DropIndex(
                name: "IX_Activities_ApplicationUserId",
                table: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_OpportunityId",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Opportunities");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "FollowUps");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "FollowUps");

            migrationBuilder.DropColumn(
                name: "OpportunityId",
                table: "FollowUps");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "OpportunityId",
                table: "Activities");

            migrationBuilder.RenameColumn(
                name: "EstimatedValue",
                table: "Leads",
                newName: "ExpectedValue");

            migrationBuilder.RenameColumn(
                name: "ScheduledDate",
                table: "FollowUps",
                newName: "FollowUpDate");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "FollowUps",
                newName: "Remarks");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "Customers",
                newName: "State");

            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "AuditLogs",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "OldValues",
                table: "AuditLogs",
                newName: "OldValue");

            migrationBuilder.RenameColumn(
                name: "NewValues",
                table: "AuditLogs",
                newName: "NewValue");

            migrationBuilder.RenameColumn(
                name: "PerformedBy",
                table: "Activities",
                newName: "AssignedTo");

            migrationBuilder.AlterColumn<string>(
                name: "OpportunityName",
                table: "Opportunities",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "Opportunities",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "LeadId",
                table: "Opportunities",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Opportunities",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Leads",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "LeadName",
                table: "Leads",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Leads",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "LeadCode",
                table: "Leads",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Subject",
                table: "FollowUps",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Customers",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Customers",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerName",
                table: "Customers",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "CompanyName",
                table: "Customers",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Customers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Customers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CustomerCode",
                table: "Customers",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AuditLogs",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IpAddress",
                table: "AuditLogs",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(45)",
                oldMaxLength: 45,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Activities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Activities",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "Activities",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Opportunities_AssignedTo",
                table: "Opportunities",
                column: "AssignedTo");

            migrationBuilder.CreateIndex(
                name: "IX_Opportunities_LeadId",
                table: "Opportunities",
                column: "LeadId");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_AssignedTo",
                table: "Leads",
                column: "AssignedTo");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_LeadCode",
                table: "Leads",
                column: "LeadCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FollowUps_AssignedTo",
                table: "FollowUps",
                column: "AssignedTo");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CustomerCode",
                table: "Customers",
                column: "CustomerCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Email",
                table: "Customers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_OwnerId",
                table: "Customers",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Phone",
                table: "Customers",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Activities_AssignedTo",
                table: "Activities",
                column: "AssignedTo");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_AspNetUsers_AssignedTo",
                table: "Activities",
                column: "AssignedTo",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Customers_CustomerId",
                table: "Activities",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Leads_LeadId",
                table: "Activities",
                column: "LeadId",
                principalTable: "Leads",
                principalColumn: "LeadId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_AspNetUsers_OwnerId",
                table: "Customers",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_FollowUps_AspNetUsers_AssignedTo",
                table: "FollowUps",
                column: "AssignedTo",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_FollowUps_Customers_CustomerId",
                table: "FollowUps",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FollowUps_Leads_LeadId",
                table: "FollowUps",
                column: "LeadId",
                principalTable: "Leads",
                principalColumn: "LeadId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_AspNetUsers_AssignedTo",
                table: "Leads",
                column: "AssignedTo",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Opportunities_AspNetUsers_AssignedTo",
                table: "Opportunities",
                column: "AssignedTo",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Opportunities_Customers_CustomerId",
                table: "Opportunities",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Opportunities_Leads_LeadId",
                table: "Opportunities",
                column: "LeadId",
                principalTable: "Leads",
                principalColumn: "LeadId",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
