using CyberQuiz.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CyberQuiz.DAL
{
    public static class CyberQuizSeeder
    {
        public static void SeedCyberQuizData(this ModelBuilder modelBuilder)
        {
            // Seeda Categories
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Nätverkssäkerhet" },
                new Category { Id = 2, Name = "Applikationssäkerhet" },
                new Category { Id = 3, Name = "Social Engineering" }
            };

            modelBuilder.Entity<Category>().HasData(categories);

            // Seeda SubCategorier
            var subCategories = new List<SubCategory>
            {
                // Nätverkssäkerhet
                new SubCategory { Id = 1, Name = "Grundläggande Nätverk", OrderIndex = 1, CategoryId = 1 },
                new SubCategory { Id = 2, Name = "Brandväggar och IDS", OrderIndex = 2, CategoryId = 1 },
                new SubCategory { Id = 3, Name = "VPN och Kryptering", OrderIndex = 3, CategoryId = 1 },
                
                // Applikationssäkerhet
                new SubCategory { Id = 4, Name = "OWASP Top 10", OrderIndex = 1, CategoryId = 2 },
                new SubCategory { Id = 5, Name = "Säker Kodning", OrderIndex = 2, CategoryId = 2 },
                new SubCategory { Id = 6, Name = "Autentisering & Auktorisering", OrderIndex = 3, CategoryId = 2 },
                
                // Social Engineering
                new SubCategory { Id = 7, Name = "Phishing", OrderIndex = 1, CategoryId = 3 },
                new SubCategory { Id = 8, Name = "Manipulation", OrderIndex = 2, CategoryId = 3 },
                new SubCategory { Id = 9, Name = "Säkerhetsmedvetenhet", OrderIndex = 3, CategoryId = 3 }
            };

            modelBuilder.Entity<SubCategory>().HasData(subCategories);
            
            SeedQuestions(modelBuilder);
        }

        private static void SeedQuestions(ModelBuilder modelBuilder)
        {
            // Seeda frågor
            var questions = new List<Question>();
            var answerOptions = new List<AnswerOption>();
            int questionId = 1;
            int answerOptionId = 1;

            // SubCategory 1: Grundläggande Nätverk (10 questions)

            // Question 1
            questions.Add(new Question { Id = questionId, Text = "Vad står TCP för?", SubCategoryId = 1 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Transmission Control Protocol", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Transfer Control Protocol", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Transport Communication Protocol", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 2
            questions.Add(new Question { Id = questionId, Text = "Vilket protokoll används för säker webbkommunikation?", SubCategoryId = 1 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "HTTP", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "HTTPS", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "FTP", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 3
            questions.Add(new Question { Id = questionId, Text = "Vilken port använder standardmässigt HTTPS?", SubCategoryId = 1 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "80", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "443", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "8080", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 4
            questions.Add(new Question { Id = questionId, Text = "Vad är en MAC-adress?", SubCategoryId = 1 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En fysisk adress för nätverksgränssnitt", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En logisk IP-adress", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En krypteringsalgoritm", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 5
            questions.Add(new Question { Id = questionId, Text = "Vilket lager i OSI-modellen hanterar routing?", SubCategoryId = 1 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Transport Layer", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Network Layer", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Data Link Layer", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 6
            questions.Add(new Question { Id = questionId, Text = "Vad används DNS för?", SubCategoryId = 1 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Att översätta domännamn till IP-adresser", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Att kryptera nätverkstrafik", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Att hantera e-post", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 7
            questions.Add(new Question { Id = questionId, Text = "Vilket IP-adressintervall är reserverat för privata nätverk (Class C)?", SubCategoryId = 1 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "10.0.0.0 - 10.255.255.255", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "192.168.0.0 - 192.168.255.255", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "172.16.0.0 - 172.31.255.255", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 8
            questions.Add(new Question { Id = questionId, Text = "Vad är ett subnät?", SubCategoryId = 1 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En logisk uppdelning av ett IP-nätverk", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En typ av brandvägg", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Ett krypteringsprotokoll", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 9
            questions.Add(new Question { Id = questionId, Text = "Vilket protokoll används för att skicka e-post?", SubCategoryId = 1 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "SMTP", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "POP3", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "IMAP", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 10
            questions.Add(new Question { Id = questionId, Text = "Vad är ARP (Address Resolution Protocol) till för?", SubCategoryId = 1 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Att översätta IP-adresser till MAC-adresser", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Att kryptera data", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Att hantera routing", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // SubCategory 2: Brandväggar och IDS (10 questions)

            // Question 11
            questions.Add(new Question { Id = questionId, Text = "Vad är en brandväggs huvudsakliga funktion?", SubCategoryId = 2 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Att filtrera nätverkstrafik baserat på regler", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Att kryptera data", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Att scanna efter virus", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 12
            questions.Add(new Question { Id = questionId, Text = "Vad står IDS för?", SubCategoryId = 2 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Internet Detection System", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Intrusion Detection System", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Internal Defense System", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 13
            questions.Add(new Question { Id = questionId, Text = "Vad är skillnaden mellan IDS och IPS?", SubCategoryId = 2 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "IDS upptäcker hot, IPS blockerar dem aktivt", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "IDS är snabbare än IPS", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Det finns ingen skillnad", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 14
            questions.Add(new Question { Id = questionId, Text = "Vilken typ av brandvägg opererar på applikationslagret?", SubCategoryId = 2 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Packet Filter Firewall", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Application Layer Firewall (WAF)", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Stateful Firewall", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 15
            questions.Add(new Question { Id = questionId, Text = "Vad betyder 'Stateful Inspection' i en brandvägg?", SubCategoryId = 2 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Den håller reda på tillståndet för aktiva anslutningar", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Den blockerar all trafik som standard", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Den krypterar trafik automatiskt", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 16
            questions.Add(new Question { Id = questionId, Text = "Vad är en DMZ (Demilitarized Zone)?", SubCategoryId = 2 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Ett isolerat nätverk mellan internet och internt nätverk", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En typ av krypteringsalgoritm", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Ett antivirusprogram", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 17
            questions.Add(new Question { Id = questionId, Text = "Vilken metod använder en IDS för att upptäcka hot?", SubCategoryId = 2 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Signaturbaserad och anomalibaserad detektering", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Endast port-scanning", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Genom att blockera all trafik", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 18
            questions.Add(new Question { Id = questionId, Text = "Vad är en 'false positive' i ett IDS?", SubCategoryId = 2 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "När systemet felaktigt identifierar legitim trafik som hot", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "När systemet missar ett verkligt hot", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "När systemet kraschar", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 19
            questions.Add(new Question { Id = questionId, Text = "Vilken port blockerar en brandvägg ofta för att förhindra oönskad fildelning?", SubCategoryId = 2 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Port 445 (SMB)", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Port 80 (HTTP)", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Port 22 (SSH)", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 20
            questions.Add(new Question { Id = questionId, Text = "Vad är Snort?", SubCategoryId = 2 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Ett open-source IDS/IPS-system", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En typ av brandvägg", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Ett krypteringsprotokoll", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // SubCategory 3: VPN och Kryptering (10 questions)

            // Question 21
            questions.Add(new Question { Id = questionId, Text = "Vad står VPN för?", SubCategoryId = 3 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Virtual Private Network", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Virtual Public Network", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Verified Private Network", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 22
            questions.Add(new Question { Id = questionId, Text = "Vilket krypteringsprotokoll används vanligtvis av moderna VPN-lösningar?", SubCategoryId = 3 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "IPsec eller OpenVPN", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "HTTP", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "FTP", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 23
            questions.Add(new Question { Id = questionId, Text = "Vad är AES?", SubCategoryId = 3 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En symmetrisk krypteringsalgoritm", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En asymmetrisk krypteringsalgoritm", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Ett hashningsprotokoll", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 24
            questions.Add(new Question { Id = questionId, Text = "Vad är skillnaden mellan symmetrisk och asymmetrisk kryptering?", SubCategoryId = 3 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Symmetrisk använder samma nyckel, asymmetrisk använder nyckelpar", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Asymmetrisk är alltid snabbare", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Det finns ingen skillnad", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 25
            questions.Add(new Question { Id = questionId, Text = "Vilket protokoll används för att skapa säkra tunnlar i ett VPN?", SubCategoryId = 3 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "IPsec", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "DNS", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "SMTP", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 26
            questions.Add(new Question { Id = questionId, Text = "Vad är en hash-funktion?", SubCategoryId = 3 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En funktion som skapar en fix-längd digital fingeravtryck av data", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En funktion som krypterar data", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En funktion som komprimerar filer", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 27
            questions.Add(new Question { Id = questionId, Text = "Vilket är ett exempel på en hash-algoritm?", SubCategoryId = 3 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "SHA-256", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "AES", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "RSA", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 28
            questions.Add(new Question { Id = questionId, Text = "Vad används RSA-kryptering till?", SubCategoryId = 3 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Asymmetrisk kryptering och digital signering", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Endast för att komprimera data", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "För att scanna efter virus", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 29
            questions.Add(new Question { Id = questionId, Text = "Vad är ett digitalt certifikat?", SubCategoryId = 3 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Ett elektroniskt dokument som verifierar ägarens identitet", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En typ av virus", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Ett lösenord", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 30
            questions.Add(new Question { Id = questionId, Text = "Vilket protokoll används för att säkra SSL/TLS-anslutningar?", SubCategoryId = 3 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "HTTPS använder SSL/TLS", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "FTP", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Telnet", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // SubCategory 4: OWASP Top 10 (10 questions)

            // Question 31
            questions.Add(new Question { Id = questionId, Text = "Vad står OWASP för?", SubCategoryId = 4 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Open Web Application Security Project", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Online Web Application Security Protocol", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Open Wireless Access Security Project", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 32
            questions.Add(new Question { Id = questionId, Text = "Vad är SQL Injection?", SubCategoryId = 4 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En attack där skadlig SQL-kod injiceras i applikationen", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Ett sätt att optimera databaser", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En typ av kryptering", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 33
            questions.Add(new Question { Id = questionId, Text = "Vad är XSS (Cross-Site Scripting)?", SubCategoryId = 4 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En attack där skadlig JavaScript körs i offrets webbläsare", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Ett protokoll för säker kommunikation", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En typ av brandvägg", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 34
            questions.Add(new Question { Id = questionId, Text = "Hur förhindrar man SQL Injection?", SubCategoryId = 4 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Genom att använda parametriserade queries/prepared statements", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Genom att bara använda NoSQL-databaser", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Genom att inaktivera JavaScript", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 35
            questions.Add(new Question { Id = questionId, Text = "Vad är CSRF (Cross-Site Request Forgery)?", SubCategoryId = 4 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En attack där offret utför oönskade handlingar när de är inloggade", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En typ av virus", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Ett säkert autentiseringsprotokoll", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 36
            questions.Add(new Question { Id = questionId, Text = "Vad är Broken Authentication?", SubCategoryId = 4 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Svagheter i autentiserings- och sessionshantering", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "När autentisering är för stark", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En typ av krypteringsalgoritm", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 37
            questions.Add(new Question { Id = questionId, Text = "Vad är Sensitive Data Exposure?", SubCategoryId = 4 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "När känslig data inte skyddas ordentligt", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Ett protokoll för datadelning", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En typ av SQL-attack", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 38
            questions.Add(new Question { Id = questionId, Text = "Vad är Security Misconfiguration?", SubCategoryId = 4 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Felaktig eller osäker konfiguration av system och applikationer", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "När konfigurationen är för säker", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Ett krypteringsprotokoll", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 39
            questions.Add(new Question { Id = questionId, Text = "Vad är Insecure Deserialization?", SubCategoryId = 4 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "När osäker deserialisering leder till fjärrkörning av kod", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "När data serialiseras för säkert", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En typ av brandvägg", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 40
            questions.Add(new Question { Id = questionId, Text = "Vad är Insufficient Logging & Monitoring?", SubCategoryId = 4 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "När attacker inte upptäcks på grund av bristfällig loggning", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "När det finns för mycket loggning", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En typ av virus", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // SubCategory 5: Säker Kodning (10 questions)

            // Question 41
            questions.Add(new Question { Id = questionId, Text = "Vad är input validation?", SubCategoryId = 5 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Att validera och sanera all användarinput", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Att tillåta all input utan kontroll", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Att endast validera output", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 42
            questions.Add(new Question { Id = questionId, Text = "Varför ska man aldrig lagra lösenord i klartext?", SubCategoryId = 5 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Det utgör en enorm säkerhetsrisk vid dataintrång", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Det tar för mycket diskutrymme", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Det är ingen skillnad", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 43
            questions.Add(new Question { Id = questionId, Text = "Vad är 'Principle of Least Privilege'?", SubCategoryId = 5 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Ge användare/processer endast de minsta rättigheter de behöver", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Ge alla användare admin-rättigheter", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Begränsa antalet användare", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 44
            questions.Add(new Question { Id = questionId, Text = "Vad är 'Secure by Default'?", SubCategoryId = 5 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "System ska vara säkra direkt utan extra konfiguration", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Säkerhet är optional", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Säkerhet behöver alltid konfigureras manuellt", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 45
            questions.Add(new Question { Id = questionId, Text = "Vad är ett buffer overflow?", SubCategoryId = 5 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "När mer data skrivs till en buffer än den kan hantera", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "När buffer är tom", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En typ av kryptering", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 46
            questions.Add(new Question { Id = questionId, Text = "Varför är hårdkodade API-nycklar farliga?", SubCategoryId = 5 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "De kan läcka via versionskontroll och är svåra att rotera", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "De gör koden snabbare", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Det är inget problem", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 47
            questions.Add(new Question { Id = questionId, Text = "Vad är 'Defense in Depth'?", SubCategoryId = 5 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Flera lager av säkerhetskontroller", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Endast en säkerhetsmekanism", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Att dölja kod", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 48
            questions.Add(new Question { Id = questionId, Text = "Vad är 'Code Review'?", SubCategoryId = 5 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Systematisk granskning av källkod för att hitta buggar och säkerhetsproblem", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Att skriva ny kod", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Att ta bort gammal kod", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 49
            questions.Add(new Question { Id = questionId, Text = "Varför ska man använda säkra random-generatorer för kryptografiska ändamål?", SubCategoryId = 5 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Vanliga random-funktioner är förutsägbara och osäkra", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Det finns ingen skillnad", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Säkra random-generatorer är snabbare", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 50
            questions.Add(new Question { Id = questionId, Text = "Vad är 'Fail Secure'?", SubCategoryId = 5 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "System ska gå in i ett säkert tillstånd vid fel", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "System ska aldrig få fel", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "System ska öppna allt vid fel", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // SubCategory 6: Autentisering & Auktorisering (10 questions)

            // Question 51
            questions.Add(new Question { Id = questionId, Text = "Vad är skillnaden mellan autentisering och auktorisering?", SubCategoryId = 6 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Autentisering verifierar identitet, auktorisering ger åtkomsträttigheter", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Det är samma sak", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Auktorisering verifierar identitet", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 52
            questions.Add(new Question { Id = questionId, Text = "Vad är MFA (Multi-Factor Authentication)?", SubCategoryId = 6 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Autentisering med flera oberoende faktorer", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Att ha flera lösenord", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En typ av brandvägg", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 53
            questions.Add(new Question { Id = questionId, Text = "Vilka är de tre typerna av autentiseringsfaktorer?", SubCategoryId = 6 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Något du vet, något du har, något du är", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Lösenord, fingeravtryck, nätverk", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Användarnamn, e-post, telefonnummer", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 54
            questions.Add(new Question { Id = questionId, Text = "Vad är OAuth?", SubCategoryId = 6 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Ett auktoriseringsprotokoll för att delegera åtkomst", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Ett lösenordshanteringssystem", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En krypteringsalgoritm", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 55
            questions.Add(new Question { Id = questionId, Text = "Vad är JWT (JSON Web Token)?", SubCategoryId = 6 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Ett kompakt, självständigt token-format för säker informationsöverföring", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En databas", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Ett programmeringsspråk", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 56
            questions.Add(new Question { Id = questionId, Text = "Vad är Session Hijacking?", SubCategoryId = 6 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "När en angripare stjäl en användares session-token", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "När en användare loggar in flera gånger", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Ett sätt att förbättra prestanda", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 57
            questions.Add(new Question { Id = questionId, Text = "Vad är RBAC (Role-Based Access Control)?", SubCategoryId = 6 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Åtkomstkontroll baserad på användarroller", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Ett krypteringsprotokoll", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En typ av brandvägg", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 58
            questions.Add(new Question { Id = questionId, Text = "Vad är SSO (Single Sign-On)?", SubCategoryId = 6 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En gång inloggning ger åtkomst till flera system", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Endast ett lösenord för alla system", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En typ av virus", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 59
            questions.Add(new Question { Id = questionId, Text = "Varför är password hashing viktigt?", SubCategoryId = 6 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "För att skydda lösenord även om databasen komprometteras", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "För att göra inloggning snabbare", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Det är inte viktigt", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 60
            questions.Add(new Question { Id = questionId, Text = "Vad är en API-nyckel?", SubCategoryId = 6 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En unik identifierare för autentisering mot ett API", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Ett lösenord för användare", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En typ av krypteringsalgoritm", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // SubCategory 7: Phishing (10 questions)

            // Question 61
            questions.Add(new Question { Id = questionId, Text = "Vad är phishing?", SubCategoryId = 7 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En social engineering-attack som lurar offer att lämna ut känslig information", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En typ av virus", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En krypteringsmetod", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 62
            questions.Add(new Question { Id = questionId, Text = "Vad är spear phishing?", SubCategoryId = 7 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Riktad phishing mot specifika individer eller organisationer", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Phishing via telefon", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En typ av brandvägg", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 63
            questions.Add(new Question { Id = questionId, Text = "Vilka tecken tyder på ett phishing-mail?", SubCategoryId = 7 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Misstänkt avsändare, dålig grammatik, brådska, misstänkta länkar", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Välskrivet språk och officiell logotyp", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "E-post från din bank", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 64
            questions.Add(new Question { Id = questionId, Text = "Vad är whaling?", SubCategoryId = 7 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Phishing-attacker riktade mot höga chefer eller VIP:ar", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Phishing via SMS", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En typ av malware", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 65
            questions.Add(new Question { Id = questionId, Text = "Vad är smishing?", SubCategoryId = 7 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Phishing via SMS-meddelanden", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Phishing via e-post", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En typ av kryptering", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 66
            questions.Add(new Question { Id = questionId, Text = "Vad är vishing?", SubCategoryId = 7 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Phishing via telefon (röstsamtal)", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Phishing via video", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En typ av virus", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 67
            questions.Add(new Question { Id = questionId, Text = "Hur ska man hantera ett misstänkt phishing-mail?", SubCategoryId = 7 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Klicka inte på länkar, rapportera mailet och radera det", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Svara på mailet och fråga om det är äkta", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Klicka på länken för att kontrollera", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 68
            questions.Add(new Question { Id = questionId, Text = "Vad är en phishing-webbplats?", SubCategoryId = 7 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En falsk webbplats som efterliknar en legitim sajt för att stjäla inloggningsuppgifter", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En säker webbplats", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En webbplats för att köpa fisk", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 69
            questions.Add(new Question { Id = questionId, Text = "Vad är email spoofing?", SubCategoryId = 7 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Att förfalska avsändaradressen i ett e-postmeddelande", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Att kryptera e-post", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Att skicka massor av e-post", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 70
            questions.Add(new Question { Id = questionId, Text = "Vilken åtgärd skyddar bäst mot phishing?", SubCategoryId = 7 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Användarutbildning och säkerhetsmedvetenhet", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Att aldrig använda e-post", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Att byta lösenord varje dag", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // SubCategory 8: Manipulation (10 questions)

            // Question 71
            questions.Add(new Question { Id = questionId, Text = "Vad är social engineering?", SubCategoryId = 8 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Manipulation av människor för att få dem att utföra handlingar eller lämna ut information", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En typ av teknisk attack", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Ett sätt att bygga nätverk", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 72
            questions.Add(new Question { Id = questionId, Text = "Vad är pretexting?", SubCategoryId = 8 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Att skapa ett falskt scenario för att lura någon att lämna ut information", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Att skriva texter i förväg", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En typ av kryptering", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 73
            questions.Add(new Question { Id = questionId, Text = "Vad är tailgating inom säkerhet?", SubCategoryId = 8 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Att obehörigt följa efter någon genom en säker dörr", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Att köra för nära bakom en annan bil", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En typ av nätverksattack", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 74
            questions.Add(new Question { Id = questionId, Text = "Vad är baiting?", SubCategoryId = 8 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Att lämna infekterade USB-minnen eller locka med gratis saker för att sprida malware", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Att fiska i en sjö", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En typ av brandvägg", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 75
            questions.Add(new Question { Id = questionId, Text = "Vad är quid pro quo-attacker?", SubCategoryId = 8 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Att erbjuda en tjänst i utbyte mot information eller åtkomst", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En latinsk fras för kryptering", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En typ av virus", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 76
            questions.Add(new Question { Id = questionId, Text = "Vad är shoulder surfing?", SubCategoryId = 8 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Att smygtitta på någons skärm eller tangentbord för att stjäla information", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En typ av vattensport", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Ett protokoll för fildelning", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 77
            questions.Add(new Question { Id = questionId, Text = "Vad är dumpster diving?", SubCategoryId = 8 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Att leta igenom sopor efter känslig information", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En typ av dykning", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En databasoperation", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 78
            questions.Add(new Question { Id = questionId, Text = "Vilken psykologisk princip utnyttjar angripare mest i social engineering?", SubCategoryId = 8 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Förtroende, auktoritet, brådska och rädsla", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Endast teknisk kunskap", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Matematiska algoritmer", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 79
            questions.Add(new Question { Id = questionId, Text = "Vad är impersonation (utgivande)?", SubCategoryId = 8 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Att utge sig för att vara någon annan för att få tillgång till information", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En typ av krypteringsmetod", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Ett sätt att optimera databaser", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 80
            questions.Add(new Question { Id = questionId, Text = "Hur skyddar man sig bäst mot manipulation och social engineering?", SubCategoryId = 8 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Genom utbildning, skepticism och verifiering av identiteter", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Genom att installera antivirus", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Genom att aldrig prata med någon", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // SubCategory 9: Säkerhetsmedvetenhet (10 questions)

            // Question 81
            questions.Add(new Question { Id = questionId, Text = "Vad är säkerhetsmedvetenhet (security awareness)?", SubCategoryId = 9 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Kunskap och förståelse om säkerhetshot och hur man skyddar sig", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Ett antivirusprogram", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "En typ av brandvägg", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 82
            questions.Add(new Question { Id = questionId, Text = "Varför är säkerhetsutbildning för anställda viktigt?", SubCategoryId = 9 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Människor är ofta den svagaste länken i säkerhetskedjan", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Det är bara ett krav från ledningen", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Det är inte viktigt", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 83
            questions.Add(new Question { Id = questionId, Text = "Vad är en stark lösenordspolicy?", SubCategoryId = 9 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Minst 12 tecken, blandning av stora/små bokstäver, siffror och specialtecken", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Minst 4 tecken, bara bokstäver", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Samma lösenord överallt för enkelhetens skull", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 84
            questions.Add(new Question { Id = questionId, Text = "Vad ska du göra om du hittar ett USB-minne på jobbet?", SubCategoryId = 9 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Lämna in det till IT-avdelningen utan att ansluta det", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Ansluta det till din dator för att se vad det innehåller", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Kasta det direkt i soporna", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 85
            questions.Add(new Question { Id = questionId, Text = "Vad är 'Clean Desk Policy'?", SubCategoryId = 9 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Policy att låsa undan känsliga dokument när de inte används", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Att ha ett rent och snyggt skrivbord", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Att städa kontoret varje dag", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 86
            questions.Add(new Question { Id = questionId, Text = "Hur ofta bör man uppdatera sina lösenord på viktiga konton?", SubCategoryId = 9 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Regelbundet eller omedelbart vid misstanke om säkerhetsbrott", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Aldrig, om lösenordet är starkt", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Varje dag", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 87
            questions.Add(new Question { Id = questionId, Text = "Vad är 'need-to-know' principen?", SubCategoryId = 9 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Endast ge åtkomst till information som är nödvändig för jobbet", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Alla ska ha tillgång till all information", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Ett sätt att kryptera data", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 88
            questions.Add(new Question { Id = questionId, Text = "Vad ska du göra om du misstänker att ditt konto har blivit hackat?", SubCategoryId = 9 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Byt lösenord omedelbart och kontakta IT-säkerhet", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Ignorera det och hoppas att det går över", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Stänga av datorn permanent", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 89
            questions.Add(new Question { Id = questionId, Text = "Varför är det viktigt att låsa sin dator när man lämnar arbetsplatsen?", SubCategoryId = 9 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "För att förhindra obehörig åtkomst till känslig information", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Det är inte viktigt på ett säkert kontor", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "För att spara energi", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Question 90
            questions.Add(new Question { Id = questionId, Text = "Vad är den viktigaste säkerhetsåtgärden du kan ta?", SubCategoryId = 9 });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Att vara vaksam, tänka kritiskt och rapportera misstänkta aktiviteter", IsCorrect = true, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Att ha det dyraste antivirusprogrammet", IsCorrect = false, QuestionId = questionId });
            answerOptions.Add(new AnswerOption { Id = answerOptionId++, Text = "Att aldrig använda internet", IsCorrect = false, QuestionId = questionId });
            questionId++;

            // Save all questions and answers to database
            modelBuilder.Entity<Question>().HasData(questions);
            modelBuilder.Entity<AnswerOption>().HasData(answerOptions);
        }
    }
}
