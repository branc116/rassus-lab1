using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sensors.Server.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sensors",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ToSensorId = table.Column<long>(type: "INTEGER", nullable: false),
                    Ip = table.Column<int>(type: "INTEGER", nullable: false),
                    Port = table.Column<int>(type: "INTEGER", nullable: false),
                    Longditude = table.Column<long>(type: "INTEGER", nullable: false),
                    Latitude = table.Column<long>(type: "INTEGER", nullable: false),
                    LastSeen = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ToSensorId1 = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sensors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sensors_Sensors_ToSensorId",
                        column: x => x.ToSensorId,
                        principalTable: "Sensors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Sensors_Sensors_ToSensorId1",
                        column: x => x.ToSensorId1,
                        principalTable: "Sensors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Readings",
                columns: table => new
                {
                    RedingId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Temperature = table.Column<long>(type: "INTEGER", nullable: false),
                    Pressure = table.Column<long>(type: "INTEGER", nullable: false),
                    Humidity = table.Column<long>(type: "INTEGER", nullable: false),
                    CO = table.Column<long>(type: "INTEGER", nullable: false),
                    NO2 = table.Column<long>(type: "INTEGER", nullable: false),
                    SO2 = table.Column<long>(type: "INTEGER", nullable: false),
                    SensorId = table.Column<long>(type: "INTEGER", nullable: false),
                    SensorId1 = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Readings", x => x.RedingId);
                    table.ForeignKey(
                        name: "FK_Readings_Sensors_SensorId",
                        column: x => x.SensorId,
                        principalTable: "Sensors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Readings_Sensors_SensorId1",
                        column: x => x.SensorId1,
                        principalTable: "Sensors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Readings_SensorId",
                table: "Readings",
                column: "SensorId");

            migrationBuilder.CreateIndex(
                name: "IX_Readings_SensorId1",
                table: "Readings",
                column: "SensorId1");

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_ToSensorId",
                table: "Sensors",
                column: "ToSensorId");

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_ToSensorId1",
                table: "Sensors",
                column: "ToSensorId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Readings");

            migrationBuilder.DropTable(
                name: "Sensors");
        }
    }
}
