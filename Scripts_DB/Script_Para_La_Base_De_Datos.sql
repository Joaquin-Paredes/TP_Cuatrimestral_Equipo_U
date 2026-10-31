CREATE DATABASE GestionComercioDB;
GO
USE GestionComercioDB;
GO

-- Elimina la tabla anterior si ya la habías creado
DROP TABLE IF EXISTS Categorias;
GO

CREATE TABLE Categorias (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Codigo VARCHAR(20) NOT NULL,
    Nombre VARCHAR(50) NOT NULL,
    Descripcion VARCHAR(250) NULL
);
GO
USE GestionComercioDB;
GO
CREATE TABLE Marcas (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Codigo VARCHAR(20) NOT NULL,
    Nombre VARCHAR(50) NOT NULL,
);
GO