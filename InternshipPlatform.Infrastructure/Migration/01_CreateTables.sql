CREATE TABLE "Users" (
  "Id"                    SERIAL NOT NULL, 
  "Email"                 text UNIQUE, 
  "PasswordHash"          text NOT NULL, 
  "IsVerified"            bool NOT NULL, 
  "RefreshToken"          text, 
  "RefreshTokenExpiredAt" timestamp NOT NULL, 
  "RoleId"                integer NOT NULL, 
  PRIMARY KEY ("Id"));
CREATE TABLE "Roles" (
  "Id"   SERIAL NOT NULL, 
  "Name" text NOT NULL UNIQUE, 
  PRIMARY KEY ("Id"));
CREATE TABLE "StudentProfiles" (
  "UserId"         integer NOT NULL, 
  "Name"           text NOT NULL, 
  "Surname"        text NOT NULL, 
  "Patronymic"     text, 
  "BirthdayDate"   date, 
  "Phone"          varchar(12), 
  "VkLink"         text, 
  "MaxLink"         text, 
  "TgLink"         text, 
  "GithubLink"     text, 
  "University"     text, 
  "Specialization" text, 
  "GraduationYear" integer, 
  PRIMARY KEY ("UserId"));
CREATE TABLE "EmployerProfiles" (
  "UserId"    integer NOT NULL, 
  "CompanyId" integer NOT NULL, 
  PRIMARY KEY ("UserId"));
CREATE TABLE "Companies" (
  "Id"          SERIAL NOT NULL, 
  "Name"        text NOT NULL, 
  "Inn"         text NOT NULL UNIQUE, 
  "Link"        text, 
  "Description" text, 
  "LogoPath"    text, 
  PRIMARY KEY ("Id"));
CREATE TABLE "Vacancies" (
  "Id"               SERIAL NOT NULL, 
  "Title"            text NOT NULL, 
  "Description"      text, 
  "SalaryFrom"       integer, 
  "SalaryTo"         integer, 
  "IsRemote"         bool NOT NULL, 
  "City"             text, 
  "IsActive"         bool, 
  "ViewsCount"       integer, 
  "SpecializationId" integer NOT NULL, 
  "CompanyId"        integer NOT NULL, 
  PRIMARY KEY ("Id"));
CREATE TABLE "Responses" (
  "Id"                  SERIAL NOT NULL, 
  "VacancyId"           integer NOT NULL, 
  "ResumeId"            integer NOT NULL, 
  "ResponseInitiatorId" integer NOT NULL, 
  "ResponseStatusesId"  integer NOT NULL, 
  PRIMARY KEY ("Id"), 
  CONSTRAINT "unique_resumes" 
    UNIQUE ("VacancyId", "ResumeId"));
CREATE TABLE "ResponseStatuses" (
  "Id"   SERIAL NOT NULL, 
  "Name" text NOT NULL UNIQUE, 
  PRIMARY KEY ("Id"));
CREATE TABLE "ResponseInitiators" (
  "Id"   SERIAL NOT NULL, 
  "Name" text NOT NULL UNIQUE, 
  PRIMARY KEY ("Id"));
CREATE TABLE "Skills" (
  "Id"   SERIAL NOT NULL, 
  "Name" text NOT NULL UNIQUE, 
  PRIMARY KEY ("Id"));
CREATE TABLE "Categories" (
  "Id"   SERIAL NOT NULL, 
  "Name" text NOT NULL UNIQUE, 
  PRIMARY KEY ("Id"));
CREATE TABLE "Specializations" (
  "Id"         SERIAL NOT NULL, 
  "Name"       text NOT NULL UNIQUE, 
  "CategoryId" integer NOT NULL, 
  PRIMARY KEY ("Id"));
CREATE TABLE "Resumes" (
  "Id"               SERIAL NOT NULL, 
  "Description"      text NOT NULL, 
  "IsActive"         bool NOT NULL, 
  "SpecializationId" integer NOT NULL, 
  "StudentId"        integer NOT NULL, 
  PRIMARY KEY ("Id"));
CREATE TABLE "SkillsToResume" (
  "SkillId"  integer NOT NULL, 
  "ResumeId" integer NOT NULL, 
  PRIMARY KEY ("SkillId", 
  "ResumeId"));
CREATE TABLE "Chats" (
  "ResponsesId" integer NOT NULL, 
  PRIMARY KEY ("ResponsesId"));
CREATE TABLE "Messages" (
  "Id"        SERIAL NOT NULL, 
  "Content"   text NOT NULL, 
  "CreatedAt" timestamp NOT NULL, 
  "ChatId"    integer NOT NULL, 
  "SenderId"  integer NOT NULL, 
  PRIMARY KEY ("Id"));
CREATE TABLE "SubscriptionPlans" (
  "Id"                SERIAL NOT NULL, 
  "Name"              text NOT NULL, 
  "Description"       text NOT NULL, 
  "Price"             integer NOT NULL, 
  "DurationDays"      integer NOT NULL, 
  "MaxVacancies"      integer NOT NULL, 
  "VacancyPeriodDays" integer NOT NULL, 
  PRIMARY KEY ("Id"));
CREATE TABLE "SubscribtionStatuses" (
  "Id"   SERIAL NOT NULL, 
  "Name" text NOT NULL UNIQUE, 
  PRIMARY KEY ("Id"));
CREATE TABLE "Subscriptions" (
  "Id"          SERIAL NOT NULL, 
  "PeriodStart" timestamp NOT NULL, 
  "PeriodEnd"   timestamp NOT NULL, 
  "CanceledAt"  timestamp, 
  "PlanId"      integer NOT NULL, 
  "StatusId"    integer NOT NULL, 
  "UserId"      integer NOT NULL, 
  PRIMARY KEY ("Id"));
CREATE UNIQUE INDEX "Users1" 
  ON "Users" ("Email");
CREATE INDEX "Users2" 
  ON "Users" ("RefreshToken");
CREATE INDEX "EmployerProfiles1" 
  ON "EmployerProfiles" ("CompanyId");
CREATE INDEX "Vacancies1" 
  ON "Vacancies" ("CompanyId");
CREATE INDEX "Vacancies2" 
  ON "Vacancies" ("SpecializationId");
CREATE INDEX "Vacancies3" 
  ON "Vacancies" ("Title");
CREATE INDEX "Vacancies4" 
  ON "Vacancies" ("City");
CREATE INDEX "Responses1" 
  ON "Responses" ("ResumeId");
CREATE INDEX "Responses2" 
  ON "Responses" ("VacancyId");
CREATE INDEX "Specializations1" 
  ON "Specializations" ("CategoryId");
CREATE INDEX "Resumes1" 
  ON "Resumes" ("StudentId");
CREATE INDEX "Resumes2" 
  ON "Resumes" ("SpecializationId");
CREATE INDEX "Messages1" 
  ON "Messages" ("ChatId", "CreatedAt");
CREATE INDEX "Subscriptions1" 
  ON "Subscriptions" ("UserId");
ALTER TABLE "Users" ADD CONSTRAINT "FKUsers379039" FOREIGN KEY ("RoleId") REFERENCES "Roles" ("Id") ON DELETE Restrict;
ALTER TABLE "StudentProfiles" ADD CONSTRAINT "FKStudentsPr711691" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON UPDATE Cascade ON DELETE Cascade;
ALTER TABLE "EmployerProfiles" ADD CONSTRAINT "FKEmployerPr861889" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON UPDATE Cascade ON DELETE Cascade;
ALTER TABLE "EmployerProfiles" ADD CONSTRAINT "FKEmployerPr684251" FOREIGN KEY ("CompanyId") REFERENCES "Companies" ("Id") ON UPDATE Cascade ON DELETE Cascade;
ALTER TABLE "Specializations" ADD CONSTRAINT "FKSpecializa451916" FOREIGN KEY ("CategoryId") REFERENCES "Categories" ("Id");
ALTER TABLE "Vacancies" ADD CONSTRAINT "FKVacancies430548" FOREIGN KEY ("SpecializationId") REFERENCES "Specializations" ("Id");
ALTER TABLE "SkillsToResume" ADD CONSTRAINT "FKSkillsToRe373177" FOREIGN KEY ("SkillId") REFERENCES "Skills" ("Id");
ALTER TABLE "SkillsToResume" ADD CONSTRAINT "FKSkillsToRe581449" FOREIGN KEY ("ResumeId") REFERENCES "Resumes" ("Id");
ALTER TABLE "Resumes" ADD CONSTRAINT "FKResumes64954" FOREIGN KEY ("SpecializationId") REFERENCES "Specializations" ("Id");
ALTER TABLE "Vacancies" ADD CONSTRAINT "FKVacancies845540" FOREIGN KEY ("CompanyId") REFERENCES "Companies" ("Id");
ALTER TABLE "Responses" ADD CONSTRAINT "FKResponses397797" FOREIGN KEY ("VacancyId") REFERENCES "Vacancies" ("Id");
ALTER TABLE "Responses" ADD CONSTRAINT "FKResponses15717" FOREIGN KEY ("ResumeId") REFERENCES "Resumes" ("Id") ON DELETE Cascade;
ALTER TABLE "Responses" ADD CONSTRAINT "FKResponses956923" FOREIGN KEY ("ResponseInitiatorId") REFERENCES "ResponseInitiators" ("Id");
ALTER TABLE "Responses" ADD CONSTRAINT "FKResponses321884" FOREIGN KEY ("ResponseStatusesId") REFERENCES "ResponseStatuses" ("Id");
ALTER TABLE "Chats" ADD CONSTRAINT "FKChats573641" FOREIGN KEY ("ResponsesId") REFERENCES "Responses" ("Id") ON UPDATE Cascade ON DELETE Cascade;
ALTER TABLE "Messages" ADD CONSTRAINT "FKMessages297040" FOREIGN KEY ("ChatId") REFERENCES "Chats" ("ResponsesId") ON UPDATE Cascade ON DELETE Cascade;
ALTER TABLE "Subscriptions" ADD CONSTRAINT "FKSubscripti215862" FOREIGN KEY ("PlanId") REFERENCES "SubscriptionPlans" ("Id");
ALTER TABLE "Subscriptions" ADD CONSTRAINT "FKSubscripti256259" FOREIGN KEY ("StatusId") REFERENCES "SubscribtionStatuses" ("Id");
ALTER TABLE "Subscriptions" ADD CONSTRAINT "FKSubscripti192955" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id");
ALTER TABLE "Resumes" ADD CONSTRAINT "FKResumes739148" FOREIGN KEY ("StudentId") REFERENCES "Users" ("Id") ON UPDATE Cascade ON DELETE Cascade;
ALTER TABLE "Messages" ADD CONSTRAINT "FKMessages187268" FOREIGN KEY ("SenderId") REFERENCES "Users" ("Id") ON UPDATE Cascade ON DELETE Cascade;
