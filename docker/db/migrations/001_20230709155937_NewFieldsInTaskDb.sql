START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20230709155937_NewFieldsInTaskDb') THEN
    ALTER TABLE "Tasks" ADD "CreationDate" timestamp with time zone NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20230709155937_NewFieldsInTaskDb') THEN
    ALTER TABLE "Tasks" ADD "DueDate" timestamp with time zone NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20230709155937_NewFieldsInTaskDb') THEN
    ALTER TABLE "Tasks" ADD "LastChangedBy" integer NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20230709155937_NewFieldsInTaskDb') THEN
    ALTER TABLE "Tasks" ADD "StoryPoints" integer NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20230709155937_NewFieldsInTaskDb') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20230709155937_NewFieldsInTaskDb', '7.0.2');
    END IF;
END $EF$;
COMMIT;

