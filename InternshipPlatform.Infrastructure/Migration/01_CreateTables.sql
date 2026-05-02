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
  "AvatarPath"       text,
  "GroupId" integer,
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
  "SalaryFrom"       integer NOT NULL, 
  "SalaryTo"         integer NOT NULL, 
  "IsRemote"         bool NOT NULL, 
  "Region"           text, 
  "IsActive"         bool NOT NULL, 
  "MinWorkExperienceYears" integer NOT NULL,
  "SpecializationId" integer NOT NULL, 
  "CompanyId"        integer NOT NULL, 
  PRIMARY KEY ("Id"));
CREATE TABLE "FavoriteVacancies" (
	"Id" SERIAL NOT NULL,
	"StudentId" integer NOT NULL,
	"VacancyId" integer NOT NULL,
	CONSTRAINT "unique_favorites" 
	  UNIQUE ("StudentId", "VacancyId"),
	PRIMARY KEY ("Id"));
CREATE TABLE "Applications" (
  "Id"                  SERIAL NOT NULL, 
  "VacancyId"           integer NOT NULL, 
  "ResumeId"            integer NOT NULL,
  "LastStatusDate"      date NOT NULL,
  "ApplicationStatusId" integer NOT NULL,
  "ChatId"              integer NOT NULL,
  PRIMARY KEY ("Id"));
CREATE TABLE "ApplicationStatuses" (
  "Id"   SERIAL NOT NULL, 
  "Name" text NOT NULL UNIQUE, 
  PRIMARY KEY ("Id"));
CREATE TABLE "Skills" (
  "Id"   SERIAL NOT NULL, 
  "Name" text NOT NULL UNIQUE, 
  PRIMARY KEY ("Id"));
CREATE TABLE "Specializations" (
  "Id"         SERIAL NOT NULL, 
  "Name"       text NOT NULL UNIQUE,
  PRIMARY KEY ("Id"));
CREATE TABLE "Resumes" (
  "Id"               SERIAL NOT NULL,
  "LastUpdateDate"   date NOT NULL,
  "Description"      text, 
  "DesiredSalary"    integer NOT NULL,
  "Region"           text,
  "IsActive"         bool NOT NULL, 
  "SpecializationId" integer NOT NULL, 
  "StudentId"        integer NOT NULL, 
  PRIMARY KEY ("Id"));
CREATE TABLE "WorkExperiences" (
  "Id" SERIAL NOT NULL,
  "ResumeId" integer NOT NULL,
  "CompanyName" text NOT NULL,
  "Profession" text NOT NULL,
  "StartDateWork" date NOT NULL,
  "EndDateWork" date,
  "WorkDescription" text,
  PRIMARY KEY ("Id"));
CREATE TABLE "SkillsToResume" (
  "SkillId"  integer NOT NULL, 
  "ResumeId" integer NOT NULL, 
  PRIMARY KEY ("SkillId", 
  "ResumeId"));
CREATE TABLE "SkillsToVacancy" (
  "SkillId"  integer NOT NULL, 
  "VacancyId" integer NOT NULL, 
  PRIMARY KEY ("SkillId", 
  "VacancyId"));
CREATE TABLE "Chats" (
  "Id"            SERIAL NOT NULL, 
  "CompanyId"     integer NOT NULL,
  "StudentId"     integer NOT NULL,
  "VacancyId"     integer NOT NULL,
  "IsClosed"      bool NOT NULL,
  PRIMARY KEY ("Id"));
CREATE TABLE "Messages" (
  "Id"        SERIAL NOT NULL, 
  "Content"   text NOT NULL, 
  "CreatedAt" timestamp NOT NULL,
  "IsRead"    bool NOT NULL,
  "ChatId"    integer NOT NULL, 
  "SenderUserId"  integer NOT NULL, 
  PRIMARY KEY ("Id"));
CREATE TABLE "ResumeViews" (
  "Id" SERIAL NOT NULL,
  "ResumeId" integer NOT NULL,
  "CompanyId" integer NOT NULL,
  "ViewDate" timestamp NOT NULL,
  PRIMARY KEY ("Id"));
CREATE TABLE "VacancyViews" (
  "Id" SERIAL NOT NULL,
  "VacancyId" integer NOT NULL,
  "StudentId" integer NOT NULL,
  "ViewDate" timestamp NOT NULL,
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
CREATE TABLE "Universities" (
  "Id"   SERIAL NOT NULL,
  "Name" text NOT NULL UNIQUE,
  PRIMARY KEY ("Id"));
CREATE TABLE "Curators" (
  "UserId"         integer NOT NULL,
  "UniversityId" integer NOT NULL,
  "Name"           text NOT NULL, 
  "Surname"        text NOT NULL, 
  "Patronymic"     text,
  "Phone"          varchar(12), 
  "VkLink"         text, 
  "MaxLink"         text, 
  "TgLink"         text,
  PRIMARY KEY ("UserId"));
CREATE TABLE "StudentGroups" (
  "Id" SERIAL NOT NULL,
  "Name" text NOT NULL,
  "UniversityId" integer NOT NULL,
  "Specialization" text NOT NULL,
  "EnrollmentYear" integer NOT NULL,
  "GraduationYear" integer NOT NULL,
  "InviteCode" text NOT NULL UNIQUE,
  "CuratorId" integer NOT NULL,
  CONSTRAINT "unique_university_group" 
	  UNIQUE ("Name", "UniversityId"),
  PRIMARY KEY ("Id"));
CREATE TABLE "StudentGroupRequests" (
  "Id" SERIAL NOT NULL,
  "GroupId" integer NOT NULL,
  "StudentId" integer NOT NULL UNIQUE,
  "CreatedAt" timestamp NOT NULL,
  PRIMARY KEY ("Id"));
CREATE UNIQUE INDEX "Users1" 
  ON "Users" ("Email");
CREATE INDEX "Users2" 
  ON "Users" ("RefreshToken");
CREATE INDEX "StudentProfiles1"
  ON "StudentProfiles" ("GroupId");
CREATE INDEX "EmployerProfiles1" 
  ON "EmployerProfiles" ("CompanyId");
CREATE INDEX "Vacancies1" 
  ON "Vacancies" ("CompanyId");
CREATE INDEX "Vacancies2" 
  ON "Vacancies" ("SpecializationId");
CREATE INDEX "Vacancies3" 
  ON "Vacancies" ("Title");
CREATE INDEX "Vacancies4" 
  ON "Vacancies" ("Region");
CREATE INDEX "Applications1" 
  ON "Applications" ("ResumeId");
CREATE INDEX "Applications2" 
  ON "Applications" ("VacancyId");
CREATE INDEX "Resumes1" 
  ON "Resumes" ("StudentId");
CREATE INDEX "Resumes2" 
  ON "Resumes" ("SpecializationId");
CREATE INDEX "Messages1" 
  ON "Messages" ("ChatId", "CreatedAt");
CREATE INDEX "Subscriptions1" 
  ON "Subscriptions" ("UserId");
CREATE INDEX "WorkExperiences1" 
  ON "WorkExperiences" ("ResumeId");
CREATE INDEX "Chats1" 
  ON "Chats" ("CompanyId");
CREATE INDEX "Chats2" 
  ON "Chats" ("StudentId");
CREATE INDEX "FavoriteVacancies1"
  ON "FavoriteVacancies" ("StudentId");
CREATE INDEX "ResumeViews1"
  ON "ResumeViews" ("ResumeId");
CREATE INDEX "ResumeViews2"
  ON "ResumeViews" ("CompanyId");
CREATE INDEX "VacancyViews1"
  ON "VacancyViews" ("VacancyId");
CREATE INDEX "VacancyViews2"
  ON "VacancyViews" ("StudentId");
CREATE INDEX "StudentGroups1"
  ON "StudentGroups" ("CuratorId");
CREATE INDEX "StudentGroupRequests1"
  ON "StudentGroupRequests" ("GroupId");
ALTER TABLE "Users" ADD CONSTRAINT "FKUsers379039" FOREIGN KEY ("RoleId") REFERENCES "Roles" ("Id") ON DELETE Restrict;
ALTER TABLE "StudentProfiles" ADD CONSTRAINT "FKStudentsPr711691" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON UPDATE Cascade ON DELETE Cascade;
ALTER TABLE "StudentProfiles" ADD CONSTRAINT "FKStudentsPr711692" FOREIGN KEY ("GroupId") REFERENCES "StudentGroups" ("Id");
ALTER TABLE "EmployerProfiles" ADD CONSTRAINT "FKEmployerPr861889" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON UPDATE Cascade ON DELETE Cascade;
ALTER TABLE "EmployerProfiles" ADD CONSTRAINT "FKEmployerPr684251" FOREIGN KEY ("CompanyId") REFERENCES "Companies" ("Id") ON UPDATE Cascade ON DELETE Cascade;
ALTER TABLE "Vacancies" ADD CONSTRAINT "FKVacancies430548" FOREIGN KEY ("SpecializationId") REFERENCES "Specializations" ("Id");
ALTER TABLE "SkillsToResume" ADD CONSTRAINT "FKSkillsToRe373177" FOREIGN KEY ("SkillId") REFERENCES "Skills" ("Id");
ALTER TABLE "SkillsToResume" ADD CONSTRAINT "FKSkillsToRe581449" FOREIGN KEY ("ResumeId") REFERENCES "Resumes" ("Id") ON UPDATE Cascade ON DELETE Cascade;
ALTER TABLE "SkillsToVacancy" ADD CONSTRAINT "FKSkillsToVa373177" FOREIGN KEY ("SkillId") REFERENCES "Skills" ("Id");
ALTER TABLE "SkillsToVacancy" ADD CONSTRAINT "FKSkillsToVa581449" FOREIGN KEY ("VacancyId") REFERENCES "Vacancies" ("Id") ON UPDATE Cascade ON DELETE Cascade;
ALTER TABLE "Resumes" ADD CONSTRAINT "FKResumes64954" FOREIGN KEY ("SpecializationId") REFERENCES "Specializations" ("Id");
ALTER TABLE "Vacancies" ADD CONSTRAINT "FKVacancies845540" FOREIGN KEY ("CompanyId") REFERENCES "Companies" ("Id") ON UPDATE Cascade ON DELETE Cascade;
ALTER TABLE "Applications" ADD CONSTRAINT "FKApplications397797" FOREIGN KEY ("VacancyId") REFERENCES "Vacancies" ("Id") ON UPDATE Cascade ON DELETE Cascade;
ALTER TABLE "Applications" ADD CONSTRAINT "FKApplications15717" FOREIGN KEY ("ResumeId") REFERENCES "Resumes" ("Id") ON UPDATE Cascade ON DELETE Cascade;
ALTER TABLE "Applications" ADD CONSTRAINT "FKApplications321884" FOREIGN KEY ("ApplicationStatusId") REFERENCES "ApplicationStatuses" ("Id");
ALTER TABLE "Applications" ADD CONSTRAINT "FKApplications321885" FOREIGN KEY ("ChatId") REFERENCES "Chats" ("Id") ON UPDATE Cascade ON DELETE Cascade;
ALTER TABLE "Chats" ADD CONSTRAINT "FKChats573641" FOREIGN KEY ("CompanyId") REFERENCES "Companies" ("Id") ON UPDATE Cascade ON DELETE Cascade;
ALTER TABLE "Chats" ADD CONSTRAINT "FKChats573642" FOREIGN KEY ("VacancyId") REFERENCES "Vacancies" ("Id") ON UPDATE Cascade ON DELETE Cascade;
ALTER TABLE "Chats" ADD CONSTRAINT "FKChats573643" FOREIGN KEY ("StudentId") REFERENCES "StudentProfiles" ("UserId") ON UPDATE Cascade ON DELETE Cascade;
ALTER TABLE "Messages" ADD CONSTRAINT "FKMessages297040" FOREIGN KEY ("ChatId") REFERENCES "Chats" ("Id") ON UPDATE Cascade ON DELETE Cascade;
ALTER TABLE "Subscriptions" ADD CONSTRAINT "FKSubscripti215862" FOREIGN KEY ("PlanId") REFERENCES "SubscriptionPlans" ("Id");
ALTER TABLE "Subscriptions" ADD CONSTRAINT "FKSubscripti256259" FOREIGN KEY ("StatusId") REFERENCES "SubscribtionStatuses" ("Id");
ALTER TABLE "Subscriptions" ADD CONSTRAINT "FKSubscripti192955" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id");
ALTER TABLE "Resumes" ADD CONSTRAINT "FKResumes739148" FOREIGN KEY ("StudentId") REFERENCES "StudentProfiles" ("UserId") ON UPDATE Cascade ON DELETE Cascade;
ALTER TABLE "Messages" ADD CONSTRAINT "FKMessages187268" FOREIGN KEY ("SenderUserId") REFERENCES "Users" ("Id") ON UPDATE Cascade ON DELETE Cascade;
ALTER TABLE "WorkExperiences" ADD CONSTRAINT "FKWorkExperiences12341" FOREIGN KEY ("ResumeId") REFERENCES "Resumes" ("Id") ON UPDATE Cascade ON DELETE Cascade;
ALTER TABLE "FavoriteVacancies" ADD CONSTRAINT "FKFavoriteVacancies573642" FOREIGN KEY ("VacancyId") REFERENCES "Vacancies" ("Id") ON UPDATE Cascade ON DELETE Cascade;
ALTER TABLE "FavoriteVacancies" ADD CONSTRAINT "FKFavoriteVacancies573643" FOREIGN KEY ("StudentId") REFERENCES "StudentProfiles" ("UserId") ON UPDATE Cascade ON DELETE Cascade;
ALTER TABLE "ResumeViews" ADD CONSTRAINT "FKResumeViews573641" FOREIGN KEY ("CompanyId") REFERENCES "Companies" ("Id") ON UPDATE Cascade ON DELETE Cascade;
ALTER TABLE "ResumeViews" ADD CONSTRAINT "FKResumeViews15717" FOREIGN KEY ("ResumeId") REFERENCES "Resumes" ("Id") ON UPDATE Cascade ON DELETE Cascade;
ALTER TABLE "VacancyViews" ADD CONSTRAINT "FKVacancyViews573643" FOREIGN KEY ("StudentId") REFERENCES "StudentProfiles" ("UserId") ON UPDATE Cascade ON DELETE Cascade;
ALTER TABLE "VacancyViews" ADD CONSTRAINT "FKVacancyViews573642" FOREIGN KEY ("VacancyId") REFERENCES "Vacancies" ("Id") ON UPDATE Cascade ON DELETE Cascade;
ALTER TABLE "Curators" ADD CONSTRAINT "FKCuratorsPr711691" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON UPDATE Cascade ON DELETE Cascade;
ALTER TABLE "Curators" ADD CONSTRAINT "FKCuratorsPr711692" FOREIGN KEY ("UniversityId") REFERENCES "Universities" ("Id");
ALTER TABLE "StudentGroups" ADD CONSTRAINT "FKStudentGroupsPr711691" FOREIGN KEY ("UniversityId") REFERENCES "Universities" ("Id");
ALTER TABLE "StudentGroups" ADD CONSTRAINT "FKStudentGroupsPr711692" FOREIGN KEY ("CuratorId") REFERENCES "Curators" ("UserId");
ALTER TABLE "StudentGroupRequests" ADD CONSTRAINT "FKStudentGroupRequestsPr711691" FOREIGN KEY ("GroupId") REFERENCES "StudentGroups" ("Id");
ALTER TABLE "StudentGroupRequests" ADD CONSTRAINT "FKStudentGroupRequestsPr711692" FOREIGN KEY ("StudentId") REFERENCES "StudentProfiles" ("UserId");