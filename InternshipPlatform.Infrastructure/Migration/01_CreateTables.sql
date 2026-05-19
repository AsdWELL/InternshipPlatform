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
CREATE TABLE "PracticeMaterials" (
  "Id" SERIAL NOT NULL,
  "PracticeOfferId" integer NOT NULL,
  "FileName" text NOT NULL,
  "FilePath" text NOT NULL,
  PRIMARY KEY ("Id"));
CREATE TABLE "PracticeOffers" (
  "Id"               SERIAL NOT NULL, 
  "Title"            text NOT NULL, 
  "Description"      text, 
  "IsRemote"         bool NOT NULL, 
  "Region"           text, 
  "IsActive"         bool NOT NULL, 
  "MaxStudents" integer NOT NULL,
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
CREATE TABLE "JobApplications" (
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
  PRIMARY KEY ("ResumeId",
  "SkillId"));
CREATE TABLE "SkillsToVacancy" (
  "SkillId"  integer NOT NULL, 
  "VacancyId" integer NOT NULL, 
  PRIMARY KEY ("VacancyId", 
  "SkillId"));
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
CREATE TABLE "Universities" (
  "Id"   SERIAL NOT NULL,
  "Name" text NOT NULL UNIQUE,
  PRIMARY KEY ("Id"));
CREATE TABLE "EducationalPrograms" (
  "Id"   SERIAL NOT NULL,
  "Name" text NOT NULL,
  "UniversityId" integer NOT NULL,
  "SpecializationCode" text NOT NULL,
  "GroupCode" text NOT NULL,
  "DurationYears" integer NOT NULL,
  CONSTRAINT "unique_university_program" 
	  UNIQUE ("Name", "UniversityId"),
  PRIMARY KEY ("Id"));
CREATE TABLE "Teachers" (
  "UserId"         integer NOT NULL,
  "UniversityId" integer NOT NULL,
  "Name"           text NOT NULL, 
  "Surname"        text NOT NULL, 
  "Patronymic"     text,
  "Phone"          varchar(12), 
  "VkLink"         text, 
  "MaxLink"         text, 
  "TgLink"         text,
  "AvatarPath"       text,
  PRIMARY KEY ("UserId"));
CREATE TABLE "StudentGroups" (
  "Id" SERIAL NOT NULL,
  "Name" text NOT NULL,
  "UniversityId" integer NOT NULL,
  "EducationalProgramId" integer NOT NULL,
  "EnrollmentYear" integer NOT NULL,
  "GraduationYear" integer NOT NULL,
  "InviteCode" text NOT NULL UNIQUE,
  "CuratorId" integer NOT NULL,
  CONSTRAINT "unique_university_group" 
	  UNIQUE ("Name", "UniversityId"),
  PRIMARY KEY ("Id"));
CREATE TABLE "StudentGroupApplications" (
  "Id" SERIAL NOT NULL,
  "GroupId" integer NOT NULL,
  "StudentId" integer NOT NULL UNIQUE,
  "CreatedAt" timestamp NOT NULL,
  PRIMARY KEY ("Id"));
CREATE TABLE "PracticePeriods" (
  "Id" SERIAL NOT NULL,
  "SupervisorId" integer NOT NULL,
  "EducationalProgramId" integer NOT NULL,
  "CourseNumber" integer NOT NULL,
  "AcademicYearStart" integer NOT NULL,
  "StartDate" date NOT NULL,
  "EndDate" date NOT NULL,
  PRIMARY KEY ("Id"));
CREATE TABLE "PracticePeriodsGroup" (
  "StudentGroupId" integer NOT NULL,
  "PracticePeriodId" integer NOT NULL,
  PRIMARY KEY ("StudentGroupId", 
  "PracticePeriodId"));
CREATE TABLE "PracticeApplications" (
  "Id" SERIAL NOT NULL,
  "StudentId" integer NOT NULL UNIQUE,
  "PracticeOfferId" integer NOT NULL,
  "PracticePeriodId" integer NOT NULL,
  "CreatedAt" timestamp NOT NULL,
  PRIMARY KEY ("Id"));
CREATE TABLE "StudentPractices" (
  "Id" SERIAL NOT NULL,
  "StudentId" integer NOT NULL,
  "PracticeOfferId" integer NOT NULL,
  "PracticePeriodId" integer NOT NULL,
  CONSTRAINT "unique_practice_per_period" 
	  UNIQUE ("StudentId", "PracticePeriodId"),
  PRIMARY KEY ("Id"));
CREATE TABLE "PracticeSubmissionStatuses" (
  "Id" SERIAL NOT NULL,
  "Name" text NOT NULL UNIQUE,
  PRIMARY KEY ("Id"));
CREATE TABLE "PracticeSubmissions" (
  "Id" SERIAL NOT NULL,
  "ReportFilePath" text NOT NULL,
  "ReportFileName" text NOT NULL,
  "SolutionFileName" text,
  "SolutionPath" text,
  "SolutionUrl" text,
  "UpdatedAt" timestamp NOT NULL,
  "ReviewedAt" timestamp,
  "Grade" integer,
  "StudentPracticeId" integer NOT NULL,
  "StatusId" integer NOT NULL,
  PRIMARY KEY ("Id"));
CREATE TABLE "SubmissionComments" (
  "Id" SERIAL NOT NULL,
  "Content" text NOT NULL,
  "CreatedAt" timestamp NOT NULL,
  "PracticeSubmissionId" integer NOT NULL,
  "SenderId" integer NOT NULL,
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
CREATE INDEX "PracticeOffers1" 
  ON "PracticeOffers" ("CompanyId");
CREATE INDEX "PracticeOffers2" 
  ON "PracticeOffers" ("SpecializationId");
CREATE INDEX "PracticeOffers3" 
  ON "PracticeOffers" ("Title");
CREATE INDEX "PracticeOffers4" 
  ON "PracticeOffers" ("Region");
CREATE INDEX "PracticeMaterials1"
  ON "PracticeMaterials" ("PracticeOfferId");
CREATE INDEX "Applications1" 
  ON "JobApplications" ("ResumeId");
CREATE INDEX "Applications2" 
  ON "JobApplications" ("VacancyId");
CREATE INDEX "Resumes1" 
  ON "Resumes" ("StudentId");
CREATE INDEX "Resumes2" 
  ON "Resumes" ("SpecializationId");
CREATE INDEX "Messages1" 
  ON "Messages" ("ChatId", "CreatedAt");
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
  ON "StudentGroupApplications" ("GroupId");
CREATE INDEX "EducationalPrograms1"
  ON "EducationalPrograms" ("UniversityId");
CREATE INDEX "PractisePeriods1"
  ON "PracticePeriods" ("EducationalProgramId");
CREATE INDEX "PractisePeriods2"
  ON "PracticePeriods" ("SupervisorId");
CREATE INDEX "PracticeApplications1"
  ON "PracticeApplications" ("PracticePeriodId");
CREATE INDEX "StudentPractices1"
  ON "StudentPractices" ("PracticePeriodId");
CREATE INDEX "SubmissionComments1"
  ON "SubmissionComments" ("PracticeSubmissionId");
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
ALTER TABLE "PracticeOffers" ADD CONSTRAINT "FKPracticeOffers845540" FOREIGN KEY ("CompanyId") REFERENCES "Companies" ("Id") ON UPDATE Cascade ON DELETE Cascade;
ALTER TABLE "PracticeOffers" ADD CONSTRAINT "FKPracticeOffers430548" FOREIGN KEY ("SpecializationId") REFERENCES "Specializations" ("Id");
ALTER TABLE "PracticeMaterials" ADD CONSTRAINT "FKPracticeMaterials430548" FOREIGN KEY ("PracticeOfferId") REFERENCES "PracticeOffers" ("Id");
ALTER TABLE "JobApplications" ADD CONSTRAINT "FKApplications397797" FOREIGN KEY ("VacancyId") REFERENCES "Vacancies" ("Id") ON UPDATE Cascade ON DELETE Cascade;
ALTER TABLE "JobApplications" ADD CONSTRAINT "FKApplications15717" FOREIGN KEY ("ResumeId") REFERENCES "Resumes" ("Id") ON UPDATE Cascade ON DELETE Cascade;
ALTER TABLE "JobApplications" ADD CONSTRAINT "FKApplications321884" FOREIGN KEY ("ApplicationStatusId") REFERENCES "ApplicationStatuses" ("Id");
ALTER TABLE "JobApplications" ADD CONSTRAINT "FKApplications321885" FOREIGN KEY ("ChatId") REFERENCES "Chats" ("Id") ON UPDATE Cascade ON DELETE Cascade;
ALTER TABLE "Chats" ADD CONSTRAINT "FKChats573641" FOREIGN KEY ("CompanyId") REFERENCES "Companies" ("Id") ON UPDATE Cascade ON DELETE Cascade;
ALTER TABLE "Chats" ADD CONSTRAINT "FKChats573642" FOREIGN KEY ("VacancyId") REFERENCES "Vacancies" ("Id") ON UPDATE Cascade ON DELETE Cascade;
ALTER TABLE "Chats" ADD CONSTRAINT "FKChats573643" FOREIGN KEY ("StudentId") REFERENCES "StudentProfiles" ("UserId") ON UPDATE Cascade ON DELETE Cascade;
ALTER TABLE "Messages" ADD CONSTRAINT "FKMessages297040" FOREIGN KEY ("ChatId") REFERENCES "Chats" ("Id") ON UPDATE Cascade ON DELETE Cascade;
ALTER TABLE "Resumes" ADD CONSTRAINT "FKResumes739148" FOREIGN KEY ("StudentId") REFERENCES "StudentProfiles" ("UserId") ON UPDATE Cascade ON DELETE Cascade;
ALTER TABLE "Messages" ADD CONSTRAINT "FKMessages187268" FOREIGN KEY ("SenderUserId") REFERENCES "Users" ("Id") ON UPDATE Cascade ON DELETE Cascade;
ALTER TABLE "WorkExperiences" ADD CONSTRAINT "FKWorkExperiences12341" FOREIGN KEY ("ResumeId") REFERENCES "Resumes" ("Id") ON UPDATE Cascade ON DELETE Cascade;
ALTER TABLE "FavoriteVacancies" ADD CONSTRAINT "FKFavoriteVacancies573642" FOREIGN KEY ("VacancyId") REFERENCES "Vacancies" ("Id") ON UPDATE Cascade ON DELETE Cascade;
ALTER TABLE "FavoriteVacancies" ADD CONSTRAINT "FKFavoriteVacancies573643" FOREIGN KEY ("StudentId") REFERENCES "StudentProfiles" ("UserId") ON UPDATE Cascade ON DELETE Cascade;
ALTER TABLE "ResumeViews" ADD CONSTRAINT "FKResumeViews573641" FOREIGN KEY ("CompanyId") REFERENCES "Companies" ("Id") ON UPDATE Cascade ON DELETE Cascade;
ALTER TABLE "ResumeViews" ADD CONSTRAINT "FKResumeViews15717" FOREIGN KEY ("ResumeId") REFERENCES "Resumes" ("Id") ON UPDATE Cascade ON DELETE Cascade;
ALTER TABLE "VacancyViews" ADD CONSTRAINT "FKVacancyViews573643" FOREIGN KEY ("StudentId") REFERENCES "StudentProfiles" ("UserId") ON UPDATE Cascade ON DELETE Cascade;
ALTER TABLE "VacancyViews" ADD CONSTRAINT "FKVacancyViews573642" FOREIGN KEY ("VacancyId") REFERENCES "Vacancies" ("Id") ON UPDATE Cascade ON DELETE Cascade;
ALTER TABLE "Teachers" ADD CONSTRAINT "FKTeachersPr711691" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON UPDATE Cascade ON DELETE Cascade;
ALTER TABLE "Teachers" ADD CONSTRAINT "FKTeachersPr711692" FOREIGN KEY ("UniversityId") REFERENCES "Universities" ("Id");
ALTER TABLE "EducationalPrograms" ADD CONSTRAINT "FKEducationalProgramsPr711692" FOREIGN KEY ("UniversityId") REFERENCES "Universities" ("Id");
ALTER TABLE "StudentGroups" ADD CONSTRAINT "FKStudentGroupsPr711690" FOREIGN KEY ("EducationalProgramId") REFERENCES "EducationalPrograms" ("Id");
ALTER TABLE "StudentGroups" ADD CONSTRAINT "FKStudentGroupsPr711691" FOREIGN KEY ("UniversityId") REFERENCES "Universities" ("Id");
ALTER TABLE "StudentGroups" ADD CONSTRAINT "FKStudentGroupsPr711692" FOREIGN KEY ("CuratorId") REFERENCES "Teachers" ("UserId");
ALTER TABLE "StudentGroupApplications" ADD CONSTRAINT "FKStudentGroupRequestsPr711691" FOREIGN KEY ("GroupId") REFERENCES "StudentGroups" ("Id");
ALTER TABLE "StudentGroupApplications" ADD CONSTRAINT "FKStudentGroupRequestsPr711692" FOREIGN KEY ("StudentId") REFERENCES "StudentProfiles" ("UserId");
ALTER TABLE "PracticePeriods" ADD CONSTRAINT "FKPractisePeriodsPr711692" FOREIGN KEY ("SupervisorId") REFERENCES "Teachers" ("UserId");
ALTER TABLE "PracticePeriods" ADD CONSTRAINT "FKPractisePeriodsPr711690" FOREIGN KEY ("EducationalProgramId") REFERENCES "EducationalPrograms" ("Id");
ALTER TABLE "PracticePeriodsGroup" ADD CONSTRAINT "FKPracticePeriodsGroup373177" FOREIGN KEY ("StudentGroupId") REFERENCES "StudentGroups" ("Id") ON UPDATE Cascade ON DELETE Cascade;
ALTER TABLE "PracticePeriodsGroup" ADD CONSTRAINT "FKPracticePeriodsGroup581449" FOREIGN KEY ("PracticePeriodId") REFERENCES "PracticePeriods" ("Id");
ALTER TABLE "PracticeApplications" ADD CONSTRAINT "FKPracticeApplications739148" FOREIGN KEY ("StudentId") REFERENCES "StudentProfiles" ("UserId") ON UPDATE Cascade ON DELETE Cascade;
ALTER TABLE "PracticeApplications" ADD CONSTRAINT "FKPracticeApplications430548" FOREIGN KEY ("PracticeOfferId") REFERENCES "PracticeOffers" ("Id");
ALTER TABLE "PracticeApplications" ADD CONSTRAINT "FKPracticeApplications581449" FOREIGN KEY ("PracticePeriodId") REFERENCES "PracticePeriods" ("Id");
ALTER TABLE "StudentPractices" ADD CONSTRAINT "FKStudentPractices739148" FOREIGN KEY ("StudentId") REFERENCES "StudentProfiles" ("UserId") ON UPDATE Cascade ON DELETE Cascade;
ALTER TABLE "StudentPractices" ADD CONSTRAINT "FKStudentPractices430548" FOREIGN KEY ("PracticeOfferId") REFERENCES "PracticeOffers" ("Id");
ALTER TABLE "StudentPractices" ADD CONSTRAINT "FKStudentPractices581449" FOREIGN KEY ("PracticePeriodId") REFERENCES "PracticePeriods" ("Id");
ALTER TABLE "PracticeSubmissions" ADD CONSTRAINT "FKPracticeSubmissions581449" FOREIGN KEY ("StudentPracticeId") REFERENCES "StudentPractices" ("Id");
ALTER TABLE "PracticeSubmissions" ADD CONSTRAINT "FKPracticeSubmissions581440" FOREIGN KEY ("StatusId") REFERENCES "PracticeSubmissionStatuses" ("Id");
ALTER TABLE "SubmissionComments" ADD CONSTRAINT "FKSubmissionComments581440" FOREIGN KEY ("PracticeSubmissionId") REFERENCES "PracticeSubmissions" ("Id");
ALTER TABLE "SubmissionComments" ADD CONSTRAINT "FKSubmissionComments581441" FOREIGN KEY ("SenderId") REFERENCES "Users" ("Id");