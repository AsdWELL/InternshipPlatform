CREATE OR REPLACE FUNCTION check_single_active_application_per_student_vacancy()
RETURNS TRIGGER AS
$$
DECLARE
    v_student_id integer;
BEGIN
    IF NEW."ApplicationStatusId" IN (3, 7) THEN
        RETURN NEW;
    END IF;

    SELECT r."StudentId"
    INTO v_student_id
    FROM "Resumes" r
    WHERE r."Id" = NEW."ResumeId";

    IF v_student_id IS NULL THEN
        RAISE EXCEPTION 'Резюме с Id=% не найдено', NEW."ResumeId";
    END IF;

    IF EXISTS (
        SELECT 1
        FROM "Applications" a
        JOIN "Resumes" r ON r."Id" = a."ResumeId"
        WHERE a."VacancyId" = NEW."VacancyId"
          AND r."StudentId" = v_student_id
          AND a."ApplicationStatusId" NOT IN (3, 7)
          AND a."Id" <> NEW."Id"
    ) THEN
        RAISE EXCEPTION 'Студент % уже имеет активный отклик на вакансию %',
            v_student_id, NEW."VacancyId";
    END IF;

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

DROP TRIGGER IF EXISTS trg_check_single_active_application_per_student_vacancy
ON "Applications";

CREATE TRIGGER trg_check_single_active_application_per_student_vacancy
BEFORE INSERT OR UPDATE ON "Applications"
FOR EACH ROW
EXECUTE FUNCTION check_single_active_application_per_student_vacancy();