using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changeBoosterToBanner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoosterS_Coupons_CouponId",
                table: "BoosterS");

            migrationBuilder.DropForeignKey(
                name: "FK_BoosterS_Products_ProductId",
                table: "BoosterS");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BoosterS",
                table: "BoosterS");

            migrationBuilder.RenameTable(
                name: "BoosterS",
                newName: "Banners");

            migrationBuilder.RenameIndex(
                name: "IX_BoosterS_ProductId",
                table: "Banners",
                newName: "IX_Banners_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_BoosterS_CouponId",
                table: "Banners",
                newName: "IX_Banners_CouponId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Banners",
                table: "Banners",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Banners_Coupons_CouponId",
                table: "Banners",
                column: "CouponId",
                principalTable: "Coupons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Banners_Products_ProductId",
                table: "Banners",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Banners_Coupons_CouponId",
                table: "Banners");

            migrationBuilder.DropForeignKey(
                name: "FK_Banners_Products_ProductId",
                table: "Banners");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Banners",
                table: "Banners");

            migrationBuilder.RenameTable(
                name: "Banners",
                newName: "BoosterS");

            migrationBuilder.RenameIndex(
                name: "IX_Banners_ProductId",
                table: "BoosterS",
                newName: "IX_BoosterS_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Banners_CouponId",
                table: "BoosterS",
                newName: "IX_BoosterS_CouponId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BoosterS",
                table: "BoosterS",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BoosterS_Coupons_CouponId",
                table: "BoosterS",
                column: "CouponId",
                principalTable: "Coupons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BoosterS_Products_ProductId",
                table: "BoosterS",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
