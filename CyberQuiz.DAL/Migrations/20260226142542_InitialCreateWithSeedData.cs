using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CyberQuiz.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateWithSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderIndex = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_SubCategories_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "SubCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnswerOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnswerOptions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    AnsweredAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    AnswerOptionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserResults_AnswerOptions_AnswerOptionId",
                        column: x => x.AnswerOptionId,
                        principalTable: "AnswerOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserResults_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserResults_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Nätverkssäkerhet" },
                    { 2, "Applikationssäkerhet" },
                    { 3, "Social Engineering" }
                });

            migrationBuilder.InsertData(
                table: "SubCategories",
                columns: new[] { "Id", "CategoryId", "Name", "OrderIndex" },
                values: new object[,]
                {
                    { 1, 1, "Grundläggande Nätverk", 1 },
                    { 2, 1, "Brandväggar och IDS", 2 },
                    { 3, 1, "VPN och Kryptering", 3 },
                    { 4, 2, "OWASP Top 10", 1 },
                    { 5, 2, "Säker Kodning", 2 },
                    { 6, 2, "Autentisering & Auktorisering", 3 },
                    { 7, 3, "Phishing", 1 },
                    { 8, 3, "Manipulation", 2 },
                    { 9, 3, "Säkerhetsmedvetenhet", 3 }
                });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "SubCategoryId", "Text" },
                values: new object[,]
                {
                    { 1, 1, "Vad står TCP för?" },
                    { 2, 1, "Vilket protokoll används för säker webbkommunikation?" },
                    { 3, 1, "Vilken port använder standardmässigt HTTPS?" },
                    { 4, 1, "Vad är en MAC-adress?" },
                    { 5, 1, "Vilket lager i OSI-modellen hanterar routing?" },
                    { 6, 1, "Vad används DNS för?" },
                    { 7, 1, "Vilket IP-adressintervall är reserverat för privata nätverk (Class C)?" },
                    { 8, 1, "Vad är ett subnät?" },
                    { 9, 1, "Vilket protokoll används för att skicka e-post?" },
                    { 10, 1, "Vad är ARP (Address Resolution Protocol) till för?" },
                    { 11, 2, "Vad är en brandväggs huvudsakliga funktion?" },
                    { 12, 2, "Vad står IDS för?" },
                    { 13, 2, "Vad är skillnaden mellan IDS och IPS?" },
                    { 14, 2, "Vilken typ av brandvägg opererar på applikationslagret?" },
                    { 15, 2, "Vad betyder 'Stateful Inspection' i en brandvägg?" },
                    { 16, 2, "Vad är en DMZ (Demilitarized Zone)?" },
                    { 17, 2, "Vilken metod använder en IDS för att upptäcka hot?" },
                    { 18, 2, "Vad är en 'false positive' i ett IDS?" },
                    { 19, 2, "Vilken port blockerar en brandvägg ofta för att förhindra oönskad fildelning?" },
                    { 20, 2, "Vad är Snort?" },
                    { 21, 3, "Vad står VPN för?" },
                    { 22, 3, "Vilket krypteringsprotokoll används vanligtvis av moderna VPN-lösningar?" },
                    { 23, 3, "Vad är AES?" },
                    { 24, 3, "Vad är skillnaden mellan symmetrisk och asymmetrisk kryptering?" },
                    { 25, 3, "Vilket protokoll används för att skapa säkra tunnlar i ett VPN?" },
                    { 26, 3, "Vad är en hash-funktion?" },
                    { 27, 3, "Vilket är ett exempel på en hash-algoritm?" },
                    { 28, 3, "Vad används RSA-kryptering till?" },
                    { 29, 3, "Vad är ett digitalt certifikat?" },
                    { 30, 3, "Vilket protokoll används för att säkra SSL/TLS-anslutningar?" },
                    { 31, 4, "Vad står OWASP för?" },
                    { 32, 4, "Vad är SQL Injection?" },
                    { 33, 4, "Vad är XSS (Cross-Site Scripting)?" },
                    { 34, 4, "Hur förhindrar man SQL Injection?" },
                    { 35, 4, "Vad är CSRF (Cross-Site Request Forgery)?" },
                    { 36, 4, "Vad är Broken Authentication?" },
                    { 37, 4, "Vad är Sensitive Data Exposure?" },
                    { 38, 4, "Vad är Security Misconfiguration?" },
                    { 39, 4, "Vad är Insecure Deserialization?" },
                    { 40, 4, "Vad är Insufficient Logging & Monitoring?" },
                    { 41, 5, "Vad är input validation?" },
                    { 42, 5, "Varför ska man aldrig lagra lösenord i klartext?" },
                    { 43, 5, "Vad är 'Principle of Least Privilege'?" },
                    { 44, 5, "Vad är 'Secure by Default'?" },
                    { 45, 5, "Vad är ett buffer overflow?" },
                    { 46, 5, "Varför är hårdkodade API-nycklar farliga?" },
                    { 47, 5, "Vad är 'Defense in Depth'?" },
                    { 48, 5, "Vad är 'Code Review'?" },
                    { 49, 5, "Varför ska man använda säkra random-generatorer för kryptografiska ändamål?" },
                    { 50, 5, "Vad är 'Fail Secure'?" },
                    { 51, 6, "Vad är skillnaden mellan autentisering och auktorisering?" },
                    { 52, 6, "Vad är MFA (Multi-Factor Authentication)?" },
                    { 53, 6, "Vilka är de tre typerna av autentiseringsfaktorer?" },
                    { 54, 6, "Vad är OAuth?" },
                    { 55, 6, "Vad är JWT (JSON Web Token)?" },
                    { 56, 6, "Vad är Session Hijacking?" },
                    { 57, 6, "Vad är RBAC (Role-Based Access Control)?" },
                    { 58, 6, "Vad är SSO (Single Sign-On)?" },
                    { 59, 6, "Varför är password hashing viktigt?" },
                    { 60, 6, "Vad är en API-nyckel?" },
                    { 61, 7, "Vad är phishing?" },
                    { 62, 7, "Vad är spear phishing?" },
                    { 63, 7, "Vilka tecken tyder på ett phishing-mail?" },
                    { 64, 7, "Vad är whaling?" },
                    { 65, 7, "Vad är smishing?" },
                    { 66, 7, "Vad är vishing?" },
                    { 67, 7, "Hur ska man hantera ett misstänkt phishing-mail?" },
                    { 68, 7, "Vad är en phishing-webbplats?" },
                    { 69, 7, "Vad är email spoofing?" },
                    { 70, 7, "Vilken åtgärd skyddar bäst mot phishing?" },
                    { 71, 8, "Vad är social engineering?" },
                    { 72, 8, "Vad är pretexting?" },
                    { 73, 8, "Vad är tailgating inom säkerhet?" },
                    { 74, 8, "Vad är baiting?" },
                    { 75, 8, "Vad är quid pro quo-attacker?" },
                    { 76, 8, "Vad är shoulder surfing?" },
                    { 77, 8, "Vad är dumpster diving?" },
                    { 78, 8, "Vilken psykologisk princip utnyttjar angripare mest i social engineering?" },
                    { 79, 8, "Vad är impersonation (utgivande)?" },
                    { 80, 8, "Hur skyddar man sig bäst mot manipulation och social engineering?" },
                    { 81, 9, "Vad är säkerhetsmedvetenhet (security awareness)?" },
                    { 82, 9, "Varför är säkerhetsutbildning för anställda viktigt?" },
                    { 83, 9, "Vad är en stark lösenordspolicy?" },
                    { 84, 9, "Vad ska du göra om du hittar ett USB-minne på jobbet?" },
                    { 85, 9, "Vad är 'Clean Desk Policy'?" },
                    { 86, 9, "Hur ofta bör man uppdatera sina lösenord på viktiga konton?" },
                    { 87, 9, "Vad är 'need-to-know' principen?" },
                    { 88, 9, "Vad ska du göra om du misstänker att ditt konto har blivit hackat?" },
                    { 89, 9, "Varför är det viktigt att låsa sin dator när man lämnar arbetsplatsen?" },
                    { 90, 9, "Vad är den viktigaste säkerhetsåtgärden du kan ta?" }
                });

            migrationBuilder.InsertData(
                table: "AnswerOptions",
                columns: new[] { "Id", "IsCorrect", "QuestionId", "Text" },
                values: new object[,]
                {
                    { 1, true, 1, "Transmission Control Protocol" },
                    { 2, false, 1, "Transfer Control Protocol" },
                    { 3, false, 1, "Transport Communication Protocol" },
                    { 4, false, 2, "HTTP" },
                    { 5, true, 2, "HTTPS" },
                    { 6, false, 2, "FTP" },
                    { 7, false, 3, "80" },
                    { 8, true, 3, "443" },
                    { 9, false, 3, "8080" },
                    { 10, true, 4, "En fysisk adress för nätverksgränssnitt" },
                    { 11, false, 4, "En logisk IP-adress" },
                    { 12, false, 4, "En krypteringsalgoritm" },
                    { 13, false, 5, "Transport Layer" },
                    { 14, true, 5, "Network Layer" },
                    { 15, false, 5, "Data Link Layer" },
                    { 16, true, 6, "Att översätta domännamn till IP-adresser" },
                    { 17, false, 6, "Att kryptera nätverkstrafik" },
                    { 18, false, 6, "Att hantera e-post" },
                    { 19, false, 7, "10.0.0.0 - 10.255.255.255" },
                    { 20, true, 7, "192.168.0.0 - 192.168.255.255" },
                    { 21, false, 7, "172.16.0.0 - 172.31.255.255" },
                    { 22, true, 8, "En logisk uppdelning av ett IP-nätverk" },
                    { 23, false, 8, "En typ av brandvägg" },
                    { 24, false, 8, "Ett krypteringsprotokoll" },
                    { 25, true, 9, "SMTP" },
                    { 26, false, 9, "POP3" },
                    { 27, false, 9, "IMAP" },
                    { 28, true, 10, "Att översätta IP-adresser till MAC-adresser" },
                    { 29, false, 10, "Att kryptera data" },
                    { 30, false, 10, "Att hantera routing" },
                    { 31, true, 11, "Att filtrera nätverkstrafik baserat på regler" },
                    { 32, false, 11, "Att kryptera data" },
                    { 33, false, 11, "Att scanna efter virus" },
                    { 34, false, 12, "Internet Detection System" },
                    { 35, true, 12, "Intrusion Detection System" },
                    { 36, false, 12, "Internal Defense System" },
                    { 37, true, 13, "IDS upptäcker hot, IPS blockerar dem aktivt" },
                    { 38, false, 13, "IDS är snabbare än IPS" },
                    { 39, false, 13, "Det finns ingen skillnad" },
                    { 40, false, 14, "Packet Filter Firewall" },
                    { 41, true, 14, "Application Layer Firewall (WAF)" },
                    { 42, false, 14, "Stateful Firewall" },
                    { 43, true, 15, "Den håller reda på tillståndet för aktiva anslutningar" },
                    { 44, false, 15, "Den blockerar all trafik som standard" },
                    { 45, false, 15, "Den krypterar trafik automatiskt" },
                    { 46, true, 16, "Ett isolerat nätverk mellan internet och internt nätverk" },
                    { 47, false, 16, "En typ av krypteringsalgoritm" },
                    { 48, false, 16, "Ett antivirusprogram" },
                    { 49, true, 17, "Signaturbaserad och anomalibaserad detektering" },
                    { 50, false, 17, "Endast port-scanning" },
                    { 51, false, 17, "Genom att blockera all trafik" },
                    { 52, true, 18, "När systemet felaktigt identifierar legitim trafik som hot" },
                    { 53, false, 18, "När systemet missar ett verkligt hot" },
                    { 54, false, 18, "När systemet kraschar" },
                    { 55, true, 19, "Port 445 (SMB)" },
                    { 56, false, 19, "Port 80 (HTTP)" },
                    { 57, false, 19, "Port 22 (SSH)" },
                    { 58, true, 20, "Ett open-source IDS/IPS-system" },
                    { 59, false, 20, "En typ av brandvägg" },
                    { 60, false, 20, "Ett krypteringsprotokoll" },
                    { 61, true, 21, "Virtual Private Network" },
                    { 62, false, 21, "Virtual Public Network" },
                    { 63, false, 21, "Verified Private Network" },
                    { 64, true, 22, "IPsec eller OpenVPN" },
                    { 65, false, 22, "HTTP" },
                    { 66, false, 22, "FTP" },
                    { 67, true, 23, "En symmetrisk krypteringsalgoritm" },
                    { 68, false, 23, "En asymmetrisk krypteringsalgoritm" },
                    { 69, false, 23, "Ett hashningsprotokoll" },
                    { 70, true, 24, "Symmetrisk använder samma nyckel, asymmetrisk använder nyckelpar" },
                    { 71, false, 24, "Asymmetrisk är alltid snabbare" },
                    { 72, false, 24, "Det finns ingen skillnad" },
                    { 73, true, 25, "IPsec" },
                    { 74, false, 25, "DNS" },
                    { 75, false, 25, "SMTP" },
                    { 76, true, 26, "En funktion som skapar en fix-längd digital fingeravtryck av data" },
                    { 77, false, 26, "En funktion som krypterar data" },
                    { 78, false, 26, "En funktion som komprimerar filer" },
                    { 79, true, 27, "SHA-256" },
                    { 80, false, 27, "AES" },
                    { 81, false, 27, "RSA" },
                    { 82, true, 28, "Asymmetrisk kryptering och digital signering" },
                    { 83, false, 28, "Endast för att komprimera data" },
                    { 84, false, 28, "För att scanna efter virus" },
                    { 85, true, 29, "Ett elektroniskt dokument som verifierar ägarens identitet" },
                    { 86, false, 29, "En typ av virus" },
                    { 87, false, 29, "Ett lösenord" },
                    { 88, true, 30, "HTTPS använder SSL/TLS" },
                    { 89, false, 30, "FTP" },
                    { 90, false, 30, "Telnet" },
                    { 91, true, 31, "Open Web Application Security Project" },
                    { 92, false, 31, "Online Web Application Security Protocol" },
                    { 93, false, 31, "Open Wireless Access Security Project" },
                    { 94, true, 32, "En attack där skadlig SQL-kod injiceras i applikationen" },
                    { 95, false, 32, "Ett sätt att optimera databaser" },
                    { 96, false, 32, "En typ av kryptering" },
                    { 97, true, 33, "En attack där skadlig JavaScript körs i offrets webbläsare" },
                    { 98, false, 33, "Ett protokoll för säker kommunikation" },
                    { 99, false, 33, "En typ av brandvägg" },
                    { 100, true, 34, "Genom att använda parametriserade queries/prepared statements" },
                    { 101, false, 34, "Genom att bara använda NoSQL-databaser" },
                    { 102, false, 34, "Genom att inaktivera JavaScript" },
                    { 103, true, 35, "En attack där offret utför oönskade handlingar när de är inloggade" },
                    { 104, false, 35, "En typ av virus" },
                    { 105, false, 35, "Ett säkert autentiseringsprotokoll" },
                    { 106, true, 36, "Svagheter i autentiserings- och sessionshantering" },
                    { 107, false, 36, "När autentisering är för stark" },
                    { 108, false, 36, "En typ av krypteringsalgoritm" },
                    { 109, true, 37, "När känslig data inte skyddas ordentligt" },
                    { 110, false, 37, "Ett protokoll för datadelning" },
                    { 111, false, 37, "En typ av SQL-attack" },
                    { 112, true, 38, "Felaktig eller osäker konfiguration av system och applikationer" },
                    { 113, false, 38, "När konfigurationen är för säker" },
                    { 114, false, 38, "Ett krypteringsprotokoll" },
                    { 115, true, 39, "När osäker deserialisering leder till fjärrkörning av kod" },
                    { 116, false, 39, "När data serialiseras för säkert" },
                    { 117, false, 39, "En typ av brandvägg" },
                    { 118, true, 40, "När attacker inte upptäcks på grund av bristfällig loggning" },
                    { 119, false, 40, "När det finns för mycket loggning" },
                    { 120, false, 40, "En typ av virus" },
                    { 121, true, 41, "Att validera och sanera all användarinput" },
                    { 122, false, 41, "Att tillåta all input utan kontroll" },
                    { 123, false, 41, "Att endast validera output" },
                    { 124, true, 42, "Det utgör en enorm säkerhetsrisk vid dataintrång" },
                    { 125, false, 42, "Det tar för mycket diskutrymme" },
                    { 126, false, 42, "Det är ingen skillnad" },
                    { 127, true, 43, "Ge användare/processer endast de minsta rättigheter de behöver" },
                    { 128, false, 43, "Ge alla användare admin-rättigheter" },
                    { 129, false, 43, "Begränsa antalet användare" },
                    { 130, true, 44, "System ska vara säkra direkt utan extra konfiguration" },
                    { 131, false, 44, "Säkerhet är optional" },
                    { 132, false, 44, "Säkerhet behöver alltid konfigureras manuellt" },
                    { 133, true, 45, "När mer data skrivs till en buffer än den kan hantera" },
                    { 134, false, 45, "När buffer är tom" },
                    { 135, false, 45, "En typ av kryptering" },
                    { 136, true, 46, "De kan läcka via versionskontroll och är svåra att rotera" },
                    { 137, false, 46, "De gör koden snabbare" },
                    { 138, false, 46, "Det är inget problem" },
                    { 139, true, 47, "Flera lager av säkerhetskontroller" },
                    { 140, false, 47, "Endast en säkerhetsmekanism" },
                    { 141, false, 47, "Att dölja kod" },
                    { 142, true, 48, "Systematisk granskning av källkod för att hitta buggar och säkerhetsproblem" },
                    { 143, false, 48, "Att skriva ny kod" },
                    { 144, false, 48, "Att ta bort gammal kod" },
                    { 145, true, 49, "Vanliga random-funktioner är förutsägbara och osäkra" },
                    { 146, false, 49, "Det finns ingen skillnad" },
                    { 147, false, 49, "Säkra random-generatorer är snabbare" },
                    { 148, true, 50, "System ska gå in i ett säkert tillstånd vid fel" },
                    { 149, false, 50, "System ska aldrig få fel" },
                    { 150, false, 50, "System ska öppna allt vid fel" },
                    { 151, true, 51, "Autentisering verifierar identitet, auktorisering ger åtkomsträttigheter" },
                    { 152, false, 51, "Det är samma sak" },
                    { 153, false, 51, "Auktorisering verifierar identitet" },
                    { 154, true, 52, "Autentisering med flera oberoende faktorer" },
                    { 155, false, 52, "Att ha flera lösenord" },
                    { 156, false, 52, "En typ av brandvägg" },
                    { 157, true, 53, "Något du vet, något du har, något du är" },
                    { 158, false, 53, "Lösenord, fingeravtryck, nätverk" },
                    { 159, false, 53, "Användarnamn, e-post, telefonnummer" },
                    { 160, true, 54, "Ett auktoriseringsprotokoll för att delegera åtkomst" },
                    { 161, false, 54, "Ett lösenordshanteringssystem" },
                    { 162, false, 54, "En krypteringsalgoritm" },
                    { 163, true, 55, "Ett kompakt, självständigt token-format för säker informationsöverföring" },
                    { 164, false, 55, "En databas" },
                    { 165, false, 55, "Ett programmeringsspråk" },
                    { 166, true, 56, "När en angripare stjäl en användares session-token" },
                    { 167, false, 56, "När en användare loggar in flera gånger" },
                    { 168, false, 56, "Ett sätt att förbättra prestanda" },
                    { 169, true, 57, "Åtkomstkontroll baserad på användarroller" },
                    { 170, false, 57, "Ett krypteringsprotokoll" },
                    { 171, false, 57, "En typ av brandvägg" },
                    { 172, true, 58, "En gång inloggning ger åtkomst till flera system" },
                    { 173, false, 58, "Endast ett lösenord för alla system" },
                    { 174, false, 58, "En typ av virus" },
                    { 175, true, 59, "För att skydda lösenord även om databasen komprometteras" },
                    { 176, false, 59, "För att göra inloggning snabbare" },
                    { 177, false, 59, "Det är inte viktigt" },
                    { 178, true, 60, "En unik identifierare för autentisering mot ett API" },
                    { 179, false, 60, "Ett lösenord för användare" },
                    { 180, false, 60, "En typ av krypteringsalgoritm" },
                    { 181, true, 61, "En social engineering-attack som lurar offer att lämna ut känslig information" },
                    { 182, false, 61, "En typ av virus" },
                    { 183, false, 61, "En krypteringsmetod" },
                    { 184, true, 62, "Riktad phishing mot specifika individer eller organisationer" },
                    { 185, false, 62, "Phishing via telefon" },
                    { 186, false, 62, "En typ av brandvägg" },
                    { 187, true, 63, "Misstänkt avsändare, dålig grammatik, brådska, misstänkta länkar" },
                    { 188, false, 63, "Välskrivet språk och officiell logotyp" },
                    { 189, false, 63, "E-post från din bank" },
                    { 190, true, 64, "Phishing-attacker riktade mot höga chefer eller VIP:ar" },
                    { 191, false, 64, "Phishing via SMS" },
                    { 192, false, 64, "En typ av malware" },
                    { 193, true, 65, "Phishing via SMS-meddelanden" },
                    { 194, false, 65, "Phishing via e-post" },
                    { 195, false, 65, "En typ av kryptering" },
                    { 196, true, 66, "Phishing via telefon (röstsamtal)" },
                    { 197, false, 66, "Phishing via video" },
                    { 198, false, 66, "En typ av virus" },
                    { 199, true, 67, "Klicka inte på länkar, rapportera mailet och radera det" },
                    { 200, false, 67, "Svara på mailet och fråga om det är äkta" },
                    { 201, false, 67, "Klicka på länken för att kontrollera" },
                    { 202, true, 68, "En falsk webbplats som efterliknar en legitim sajt för att stjäla inloggningsuppgifter" },
                    { 203, false, 68, "En säker webbplats" },
                    { 204, false, 68, "En webbplats för att köpa fisk" },
                    { 205, true, 69, "Att förfalska avsändaradressen i ett e-postmeddelande" },
                    { 206, false, 69, "Att kryptera e-post" },
                    { 207, false, 69, "Att skicka massor av e-post" },
                    { 208, true, 70, "Användarutbildning och säkerhetsmedvetenhet" },
                    { 209, false, 70, "Att aldrig använda e-post" },
                    { 210, false, 70, "Att byta lösenord varje dag" },
                    { 211, true, 71, "Manipulation av människor för att få dem att utföra handlingar eller lämna ut information" },
                    { 212, false, 71, "En typ av teknisk attack" },
                    { 213, false, 71, "Ett sätt att bygga nätverk" },
                    { 214, true, 72, "Att skapa ett falskt scenario för att lura någon att lämna ut information" },
                    { 215, false, 72, "Att skriva texter i förväg" },
                    { 216, false, 72, "En typ av kryptering" },
                    { 217, true, 73, "Att obehörigt följa efter någon genom en säker dörr" },
                    { 218, false, 73, "Att köra för nära bakom en annan bil" },
                    { 219, false, 73, "En typ av nätverksattack" },
                    { 220, true, 74, "Att lämna infekterade USB-minnen eller locka med gratis saker för att sprida malware" },
                    { 221, false, 74, "Att fiska i en sjö" },
                    { 222, false, 74, "En typ av brandvägg" },
                    { 223, true, 75, "Att erbjuda en tjänst i utbyte mot information eller åtkomst" },
                    { 224, false, 75, "En latinsk fras för kryptering" },
                    { 225, false, 75, "En typ av virus" },
                    { 226, true, 76, "Att smygtitta på någons skärm eller tangentbord för att stjäla information" },
                    { 227, false, 76, "En typ av vattensport" },
                    { 228, false, 76, "Ett protokoll för fildelning" },
                    { 229, true, 77, "Att leta igenom sopor efter känslig information" },
                    { 230, false, 77, "En typ av dykning" },
                    { 231, false, 77, "En databasoperation" },
                    { 232, true, 78, "Förtroende, auktoritet, brådska och rädsla" },
                    { 233, false, 78, "Endast teknisk kunskap" },
                    { 234, false, 78, "Matematiska algoritmer" },
                    { 235, true, 79, "Att utge sig för att vara någon annan för att få tillgång till information" },
                    { 236, false, 79, "En typ av krypteringsmetod" },
                    { 237, false, 79, "Ett sätt att optimera databaser" },
                    { 238, true, 80, "Genom utbildning, skepticism och verifiering av identiteter" },
                    { 239, false, 80, "Genom att installera antivirus" },
                    { 240, false, 80, "Genom att aldrig prata med någon" },
                    { 241, true, 81, "Kunskap och förståelse om säkerhetshot och hur man skyddar sig" },
                    { 242, false, 81, "Ett antivirusprogram" },
                    { 243, false, 81, "En typ av brandvägg" },
                    { 244, true, 82, "Människor är ofta den svagaste länken i säkerhetskedjan" },
                    { 245, false, 82, "Det är bara ett krav från ledningen" },
                    { 246, false, 82, "Det är inte viktigt" },
                    { 247, true, 83, "Minst 12 tecken, blandning av stora/små bokstäver, siffror och specialtecken" },
                    { 248, false, 83, "Minst 4 tecken, bara bokstäver" },
                    { 249, false, 83, "Samma lösenord överallt för enkelhetens skull" },
                    { 250, true, 84, "Lämna in det till IT-avdelningen utan att ansluta det" },
                    { 251, false, 84, "Ansluta det till din dator för att se vad det innehåller" },
                    { 252, false, 84, "Kasta det direkt i soporna" },
                    { 253, true, 85, "Policy att låsa undan känsliga dokument när de inte används" },
                    { 254, false, 85, "Att ha ett rent och snyggt skrivbord" },
                    { 255, false, 85, "Att städa kontoret varje dag" },
                    { 256, true, 86, "Regelbundet eller omedelbart vid misstanke om säkerhetsbrott" },
                    { 257, false, 86, "Aldrig, om lösenordet är starkt" },
                    { 258, false, 86, "Varje dag" },
                    { 259, true, 87, "Endast ge åtkomst till information som är nödvändig för jobbet" },
                    { 260, false, 87, "Alla ska ha tillgång till all information" },
                    { 261, false, 87, "Ett sätt att kryptera data" },
                    { 262, true, 88, "Byt lösenord omedelbart och kontakta IT-säkerhet" },
                    { 263, false, 88, "Ignorera det och hoppas att det går över" },
                    { 264, false, 88, "Stänga av datorn permanent" },
                    { 265, true, 89, "För att förhindra obehörig åtkomst till känslig information" },
                    { 266, false, 89, "Det är inte viktigt på ett säkert kontor" },
                    { 267, false, 89, "För att spara energi" },
                    { 268, true, 90, "Att vara vaksam, tänka kritiskt och rapportera misstänkta aktiviteter" },
                    { 269, false, 90, "Att ha det dyraste antivirusprogrammet" },
                    { 270, false, 90, "Att aldrig använda internet" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnswerOptions_QuestionId",
                table: "AnswerOptions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_SubCategoryId",
                table: "Questions",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_CategoryId",
                table: "SubCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserResults_AnswerOptionId",
                table: "UserResults",
                column: "AnswerOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserResults_QuestionId",
                table: "UserResults",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserResults_UserId",
                table: "UserResults",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "UserResults");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AnswerOptions");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "SubCategories");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
