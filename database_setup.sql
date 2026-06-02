-- =========================================================================
-- SCRIPT COMPLETO DE CONFIGURACIÓN DE BASE DE DATOS Y OBJETOS (STORED PROCEDURES)
-- Sistema: MedicalApi
-- =========================================================================

-- -------------------------------------------------------------------------
-- 1. CREACIÓN DE TABLAS PRINCIPALES
-- -------------------------------------------------------------------------

CREATE TABLE Especialidad (
    EspecialidadId INT IDENTITY PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL
);

CREATE TABLE Paciente (
    PacienteId INT IDENTITY PRIMARY KEY,
    Nombre VARCHAR(200) NOT NULL,
    RUT VARCHAR(20) NOT NULL UNIQUE,
    FechaNacimiento DATE NOT NULL
);

CREATE TABLE Doctor (
    DoctorId INT IDENTITY PRIMARY KEY,
    Nombre VARCHAR(200) NOT NULL,
    EspecialidadId INT NOT NULL,
    FOREIGN KEY (EspecialidadId)
        REFERENCES Especialidad(EspecialidadId)
);

CREATE TABLE Atencion (
    AtencionId INT IDENTITY PRIMARY KEY,
    PacienteId INT NOT NULL,
    DoctorId INT NOT NULL,
    FechaHoraInicio DATETIME NOT NULL,
    FechaHoraFin DATETIME NOT NULL,
    Diagnostico VARCHAR(MAX),

    FOREIGN KEY(PacienteId)
        REFERENCES Paciente(PacienteId),

    FOREIGN KEY(DoctorId)
        REFERENCES Doctor(DoctorId)
);

-- -------------------------------------------------------------------------
-- 2. RESTRICCIONES E ÍNDICES ADICIONALES
-- -------------------------------------------------------------------------

ALTER TABLE Paciente
ADD CONSTRAINT UQ_Paciente_RUT
UNIQUE (RUT);

ALTER TABLE Atencion
ADD CONSTRAINT CK_Atencion_Fechas
CHECK (FechaHoraFin > FechaHoraInicio);

CREATE INDEX IX_Atencion_Doctor
ON Atencion(DoctorId);

CREATE INDEX IX_Atencion_Paciente
ON Atencion(PacienteId);

CREATE INDEX IX_Atencion_FechaInicio
ON Atencion(FechaHoraInicio);

ALTER TABLE Doctor
ADD CONSTRAINT UQ_Doctor_Nombre UNIQUE (Nombre);

-- -------------------------------------------------------------------------
-- 3. INSERCIÓN DE DATOS SEMILLA (INITIAL DATA)
-- -------------------------------------------------------------------------

INSERT INTO Especialidad (Nombre)
VALUES
('Cardiología'),
('Pediatría'),
('Traumatología'),
('Neurología');

INSERT INTO Doctor (Nombre, EspecialidadId)
VALUES
('Carlos Soto', 1),
('María González', 2),
('Pedro Fuentes', 3),
('Ana Díaz', 4);

INSERT INTO Paciente (Nombre, RUT, FechaNacimiento)
VALUES
('Juan Pérez', '11111111-1', '1990-05-10'),
('María Torres', '22222222-2', '1988-08-15'),
('Pedro González', '33333333-3', '2000-03-22');

INSERT INTO Atencion (PacienteId, DoctorId, FechaHoraInicio, FechaHoraFin, Diagnostico)
VALUES
(1, 1, '2026-06-01 09:00', '2026-06-01 09:30', 'Control cardiológico'),
(2, 2, '2026-06-01 10:00', '2026-06-01 10:45', 'Control pediátrico');
GO

-- -------------------------------------------------------------------------
-- 4. PROCEDIMIENTOS ALMACENADOS (STORED PROCEDURES)
-- -------------------------------------------------------------------------

-- =========================================================================
-- A. PROCEDIMIENTOS DE PACIENTES
-- =========================================================================

CREATE OR ALTER PROC sp_Paciente_GetAll
AS
BEGIN
    SELECT * FROM Paciente
END
GO

CREATE OR ALTER PROC sp_Paciente_Insert
(
    @Nombre VARCHAR(200),
    @RUT VARCHAR(20),
    @FechaNacimiento DATE
)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Paciente WHERE RUT = @RUT)
    BEGIN
        THROW 50005, 'El RUT ya existe', 1;
    END

    INSERT INTO Paciente (Nombre, RUT, FechaNacimiento)
    VALUES (@Nombre, @RUT, @FechaNacimiento);
END
GO

CREATE OR ALTER PROC sp_Paciente_GetById
(
    @PacienteId INT
)
AS
BEGIN
    SELECT * FROM Paciente WHERE PacienteId = @PacienteId
END
GO

CREATE OR ALTER PROC sp_Paciente_Update
(
    @PacienteId INT,
    @Nombre VARCHAR(200),
    @RUT VARCHAR(20),
    @FechaNacimiento DATE
)
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Paciente WHERE PacienteId = @PacienteId)
    BEGIN
        THROW 50005, 'Paciente no existe', 1;
    END

    UPDATE Paciente
    SET Nombre = @Nombre,
        RUT = @RUT,
        FechaNacimiento = @FechaNacimiento
    WHERE PacienteId = @PacienteId;
END
GO

CREATE OR ALTER PROC sp_Paciente_Delete
(
    @PacienteId INT
)
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Paciente WHERE PacienteId = @PacienteId)
    BEGIN
        THROW 50005, 'Paciente no existe', 1;
    END

    DELETE FROM Paciente WHERE PacienteId = @PacienteId;
END
GO

CREATE OR ALTER PROC sp_Paciente_GetByRut
(
    @RUT VARCHAR(20)
)
AS
BEGIN
    SELECT * FROM Paciente WHERE RUT = @RUT
END
GO

CREATE OR ALTER PROC sp_Paciente_ExisteRut
(
    @PacienteId INT,
    @RUT VARCHAR(20)
)
AS
BEGIN
    SELECT * FROM Paciente WHERE RUT = @RUT AND PacienteId <> @PacienteId
END
GO

CREATE OR ALTER PROC sp_Paciente_Buscar
(
    @Rut VARCHAR(20) = NULL,
    @Nombre VARCHAR(200) = NULL,
    @EdadMin INT = NULL,
    @EdadMax INT = NULL
)
AS
BEGIN
    SELECT *
    FROM Paciente
    WHERE
        (@Rut IS NULL OR RUT = @Rut)
        AND (@Nombre IS NULL OR Nombre LIKE '%' + @Nombre + '%')
        AND (
            @EdadMin IS NULL OR
            DATEDIFF(YEAR, FechaNacimiento, GETDATE()) >= @EdadMin
        )
        AND (
            @EdadMax IS NULL OR
            DATEDIFF(YEAR, FechaNacimiento, GETDATE()) <= @EdadMax
        )
END
GO

-- =========================================================================
-- B. PROCEDIMIENTOS DE ESPECIALIDADES
-- =========================================================================

CREATE OR ALTER PROC sp_Especialidad_GetAll
AS
BEGIN
    SELECT * FROM Especialidad
END
GO

CREATE OR ALTER PROC sp_Especialidad_Insert
(
    @Nombre VARCHAR(200)
)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Especialidad WHERE Nombre = @Nombre)
    BEGIN
        THROW 50005, 'La especialidad ya existe', 1;
    END

    INSERT INTO Especialidad (Nombre) VALUES (@Nombre);
END
GO

CREATE OR ALTER PROC sp_Especialidad_GetById
(
    @EspecialidadId INT
)
AS
BEGIN
    SELECT * FROM Especialidad WHERE EspecialidadId = @EspecialidadId
END
GO

CREATE OR ALTER PROC sp_Especialidad_Update
(
    @EspecialidadId INT,
    @Nombre VARCHAR(200)
)
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Especialidad WHERE EspecialidadId = @EspecialidadId)
    BEGIN
        THROW 50005, 'La especialidad no existe', 1;
    END

    UPDATE Especialidad
    SET Nombre = @Nombre
    WHERE EspecialidadId = @EspecialidadId;
END
GO

CREATE OR ALTER PROC sp_Especialidad_Delete
(
    @EspecialidadId INT
)
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Especialidad WHERE EspecialidadId = @EspecialidadId)
    BEGIN
        THROW 50005, 'La especialidad no existe', 1;
    END

    DELETE FROM Especialidad WHERE EspecialidadId = @EspecialidadId;
END
GO

CREATE OR ALTER PROC sp_Especialidad_GetByName
(
    @Nombre VARCHAR(100)
)
AS
BEGIN
    SELECT * FROM Especialidad WHERE Nombre LIKE '%' + @Nombre + '%'
END
GO

CREATE OR ALTER PROC sp_Especialidad_ExisteName
(
    @EspecialidadId INT,
    @Nombre VARCHAR(100)
)
AS
BEGIN
    SELECT * FROM Especialidad WHERE Nombre = @Nombre AND EspecialidadId <> @EspecialidadId
END
GO

-- =========================================================================
-- C. PROCEDIMIENTOS DE DOCTORES
-- =========================================================================

CREATE OR ALTER PROC sp_Doctor_GetAll
AS
BEGIN
    SELECT
        d.DoctorId,
        d.Nombre,
        d.EspecialidadId,
        e.Nombre AS EspecialidadNombre
    FROM Doctor d
    INNER JOIN Especialidad e
        ON d.EspecialidadId = e.EspecialidadId
END
GO

CREATE OR ALTER PROC sp_Doctor_Insert
(
    @Nombre VARCHAR(200),
    @EspecialidadId INT
)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Doctor WHERE Nombre = @Nombre)
    BEGIN
        THROW 50005, 'El doctor ya existe', 1;
    END

    IF NOT EXISTS (SELECT 1 FROM Especialidad WHERE EspecialidadId = @EspecialidadId)
    BEGIN
        THROW 50005, 'La especialidad no existe', 1;
    END

    INSERT INTO Doctor (Nombre, EspecialidadId)
    VALUES (@Nombre, @EspecialidadId);
END
GO

CREATE OR ALTER PROC sp_Doctor_Update
(
    @DoctorId INT,
    @Nombre VARCHAR(200),
    @EspecialidadId INT
)
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Doctor WHERE DoctorId = @DoctorId)
    BEGIN
        THROW 50005, 'El doctor no existe', 1;
    END

    UPDATE Doctor
    SET Nombre = @Nombre,
        EspecialidadId = @EspecialidadId
    WHERE DoctorId = @DoctorId;
END
GO

CREATE OR ALTER PROC sp_Doctor_Delete
(
    @DoctorId INT
)
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Doctor WHERE DoctorId = @DoctorId)
    BEGIN
        THROW 50005, 'El doctor no existe', 1;
    END

    DELETE FROM Doctor WHERE DoctorId = @DoctorId;
END
GO

CREATE OR ALTER PROC sp_Doctor_ExisteName
(
    @DoctorId INT,
    @Nombre VARCHAR(200)
)
AS
BEGIN
    SELECT * FROM Doctor WHERE Nombre = @Nombre AND DoctorId <> @DoctorId
END
GO

CREATE OR ALTER PROC sp_Doctor_ExisteName_Create
(
    @Nombre VARCHAR(200)
)
AS
BEGIN
    SELECT * FROM Doctor WHERE Nombre = @Nombre
END
GO

CREATE OR ALTER PROC sp_Doctor_GetById
(
    @DoctorId INT
)
AS
BEGIN
    SELECT
        d.DoctorId,
        d.Nombre,
        d.EspecialidadId,
        e.Nombre AS EspecialidadNombre
    FROM Doctor d
    INNER JOIN Especialidad e
        ON d.EspecialidadId = e.EspecialidadId
    WHERE d.DoctorId = @DoctorId
END
GO

-- =========================================================================
-- D. PROCEDIMIENTOS DE ATENCIONES MÉDICAS
-- =========================================================================

CREATE OR ALTER PROC sp_Atencion_GetAll
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        a.AtencionId,
        a.PacienteId,
        a.DoctorId,
        a.FechaHoraInicio,
        a.FechaHoraFin,
        a.Diagnostico,
        d.Nombre AS DoctorNombre,
        d.EspecialidadId,
        e.Nombre AS EspecialidadNombre,
        p.Nombre AS PacienteNombre
    FROM Atencion a
    INNER JOIN Doctor d
        ON a.DoctorId = d.DoctorId
    INNER JOIN Especialidad e
        ON d.EspecialidadId = e.EspecialidadId
    INNER JOIN Paciente p
        ON a.PacienteId = p.PacienteId;
END
GO

CREATE OR ALTER PROC sp_Atencion_GetById
(
    @AtencionId INT
)
AS
BEGIN
    SELECT 
        a.AtencionId,
        a.PacienteId,
        a.DoctorId,
        a.FechaHoraInicio,
        a.FechaHoraFin,
        a.Diagnostico,
        d.Nombre AS DoctorNombre,
        d.EspecialidadId,
        e.Nombre AS EspecialidadNombre,
        p.Nombre AS PacienteNombre
    FROM Atencion a
    INNER JOIN Doctor d
        ON a.DoctorId = d.DoctorId
    INNER JOIN Especialidad e
        ON d.EspecialidadId = e.EspecialidadId
    INNER JOIN Paciente p
        ON a.PacienteId = p.PacienteId
    WHERE AtencionId = @AtencionId;
END
GO

CREATE OR ALTER PROC sp_Atencion_Update
(
    @AtencionId INT,
    @PacienteId INT,
    @DoctorId INT,
    @FechaHoraInicio DATETIME,
    @FechaHoraFin DATETIME,
    @Diagnostico VARCHAR(200)
)
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Atencion WHERE AtencionId = @AtencionId)
    BEGIN
        THROW 50005, 'La atencion asignada no existe', 1;
    END
    IF NOT EXISTS (SELECT 1 FROM Doctor WHERE DoctorId = @DoctorId)
    BEGIN
        THROW 50005, 'El doctor no existe', 1;
    END
    IF NOT EXISTS (SELECT 1 FROM Paciente WHERE PacienteId = @PacienteId)
    BEGIN
        THROW 50005, 'El Paciente no existe', 1;
    END

    IF EXISTS (
        SELECT 1
        FROM Atencion
        WHERE DoctorId = @DoctorId
            AND AtencionId <> @AtencionId
            AND @FechaHoraInicio < FechaHoraFin
            AND @FechaHoraFin > FechaHoraInicio
    )
    BEGIN
        THROW 50005, 'El doctor ya tiene una atencion en ese horario', 1;
    END

    UPDATE Atencion
    SET PacienteId = @PacienteId,
        DoctorId = @DoctorId,
        FechaHoraInicio = @FechaHoraInicio,
        FechaHoraFin = @FechaHoraFin,
        Diagnostico = @Diagnostico
    WHERE AtencionId = @AtencionId;
END
GO

CREATE OR ALTER PROC sp_Atencion_Insert
(
    @PacienteId INT,
    @DoctorId INT,
    @FechaHoraInicio DATETIME,
    @FechaHoraFin DATETIME,
    @Diagnostico VARCHAR(200)
)
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Doctor WHERE DoctorId = @DoctorId)
    BEGIN
        THROW 50005, 'El doctor no existe', 1;
    END
    IF NOT EXISTS (SELECT 1 FROM Paciente WHERE PacienteId = @PacienteId)
    BEGIN
        THROW 50005, 'El Paciente no existe', 1;
    END

    INSERT INTO Atencion (PacienteId, DoctorId, FechaHoraInicio, FechaHoraFin, Diagnostico)
    VALUES (@PacienteId, @DoctorId, @FechaHoraInicio, @FechaHoraFin, @Diagnostico);
END
GO

CREATE OR ALTER PROC sp_Atencion_Delete
(
    @AtencionId INT
)
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Atencion WHERE AtencionId = @AtencionId)
    BEGIN
        THROW 50005, 'La atencion no existe', 1;
    END

    DELETE FROM Atencion WHERE AtencionId = @AtencionId;
END
GO

CREATE OR ALTER PROC sp_Atencion_ExisteSolapamiento
(
    @DoctorId INT,
    @FechaHoraInicio DATETIME,
    @FechaHoraFin DATETIME
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1 1 AS Existe
    FROM Atencion
    WHERE DoctorId = @DoctorId
      AND @FechaHoraInicio < FechaHoraFin
      AND @FechaHoraFin > FechaHoraInicio;
END
GO

CREATE OR ALTER PROC sp_Atencion_PromedioPorEspecialidad
AS
BEGIN
    SELECT
        e.Nombre AS Especialidad,
        AVG(DATEDIFF(MINUTE, a.FechaHoraInicio, a.FechaHoraFin)) AS DuracionPromedioMinutos
    FROM Atencion a
    INNER JOIN Doctor d
        ON a.DoctorId = d.DoctorId
    INNER JOIN Especialidad e
        ON d.EspecialidadId = e.EspecialidadId
    GROUP BY e.Nombre
END
GO

CREATE OR ALTER PROC sp_Atencion_Buscar
(
    @FechaInicio DATETIME = NULL,
    @FechaFin DATETIME = NULL,
    @DoctorId INT = NULL,
    @PacienteId INT = NULL,
    @EspecialidadId INT = NULL
)
AS
BEGIN
    SELECT 
        a.AtencionId,
        a.PacienteId,
        a.DoctorId,
        a.FechaHoraInicio,
        a.FechaHoraFin,
        a.Diagnostico,
        d.Nombre AS DoctorNombre,
        d.EspecialidadId,
        e.Nombre AS EspecialidadNombre,
        p.Nombre AS PacienteNombre
    FROM Atencion a
    INNER JOIN Doctor d
        ON a.DoctorId = d.DoctorId
    INNER JOIN Especialidad e
        ON d.EspecialidadId = e.EspecialidadId
    INNER JOIN Paciente p
        ON a.PacienteId = p.PacienteId
    WHERE
        (@DoctorId IS NULL OR a.DoctorId = @DoctorId)
        AND (@PacienteId IS NULL OR a.PacienteId = @PacienteId)
        AND (@EspecialidadId IS NULL OR d.EspecialidadId = @EspecialidadId)
        AND (
            @FechaInicio IS NULL
            OR a.FechaHoraInicio >= @FechaInicio
        )
        AND (
            @FechaFin IS NULL
            OR a.FechaHoraInicio < DATEADD(DAY, 1, @FechaFin)
        )
END
GO
