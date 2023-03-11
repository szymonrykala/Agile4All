﻿CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20230307164645_InitialMigration') THEN
    CREATE TABLE "Files" (
        "Id" integer GENERATED BY DEFAULT AS IDENTITY,
        "Name" text NOT NULL,
        "Path" text NOT NULL,
        "Modification_Date" timestamp with time zone NOT NULL,
        "User_Id" integer NOT NULL,
        "Project_Id" integer NOT NULL,
        "Task_Id" integer NOT NULL,
        CONSTRAINT "PK_Files" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20230307164645_InitialMigration') THEN
    CREATE TABLE "Messages" (
        "Id" integer GENERATED BY DEFAULT AS IDENTITY,
        "Json_Text" text NOT NULL,
        CONSTRAINT "PK_Messages" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20230307164645_InitialMigration') THEN
    CREATE TABLE "Proj_Users" (
        "Id" integer GENERATED BY DEFAULT AS IDENTITY,
        "Project_Id" integer NOT NULL,
        "User_Id" integer NOT NULL,
        CONSTRAINT "PK_Proj_Users" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20230307164645_InitialMigration') THEN
    CREATE TABLE "Projects" (
        "Id" integer GENERATED BY DEFAULT AS IDENTITY,
        "Name" text NOT NULL,
        "Description" text NOT NULL,
        CONSTRAINT "PK_Projects" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20230307164645_InitialMigration') THEN
    CREATE TABLE "Tasks" (
        "Id" integer GENERATED BY DEFAULT AS IDENTITY,
        "Name" text NOT NULL,
        "Description" text NOT NULL,
        "Status" integer NOT NULL,
        "ProjectId" integer NOT NULL,
        "UserId" integer NOT NULL,
        CONSTRAINT "PK_Tasks" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20230307164645_InitialMigration') THEN
    CREATE TABLE "Users" (
        "Id" integer GENERATED BY DEFAULT AS IDENTITY,
        "FirstName" text NOT NULL,
        "LastName" text NOT NULL,
        "Email" text NOT NULL,
        "Role" integer NOT NULL,
        "Password" text NOT NULL,
        "Hash" text NOT NULL,
        CONSTRAINT "PK_Users" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20230307164645_InitialMigration') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20230307164645_InitialMigration', '7.0.2');
    END IF;
END $EF$;
COMMIT;
