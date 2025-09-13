
-- Crear tabla de usuarios
CREATE TABLE usuarios (
    id SERIAL PRIMARY KEY,
    nombres VARCHAR(100) NOT NULL,
    apellidos VARCHAR(100) NOT NULL,
    fechanacimiento DATE NOT NULL,
    direccion VARCHAR(255) NOT NULL,
    password VARCHAR(120) NOT NULL,
    telefono VARCHAR(20) NOT NULL,
    email VARCHAR(100) NOT NULL UNIQUE,
    fechacreacion TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    fechamodificacion TIMESTAMP NULL
);

NSERT INTO usuarios(
	id, nombres, apellidos, fechanacimiento, direccion, password, telefono, email, fechacreacion)
	VALUES ("prueba", "prueba", "1996-10-08", "prueba", "$2a$11$cakTi3daBD4fOteOFYVveellrmv6EO7sya4ZrrpT/ca4ZbZiyuAxG", "78787878", "prueba@gmail.com", "2025-09-13");