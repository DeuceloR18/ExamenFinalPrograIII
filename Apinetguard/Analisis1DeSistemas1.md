Roles del sistema:

Administrador
Técnico
Sistema

HU-01 — Registrar incidente

Como administrador, quiero registrar un nuevo incidente de red con severidad, 
descripción y sitio afectado, para que quede documentado en el sistema.

Criterios:

El incidente se crea con estado Registrado
Debe tener: título, descripción, severidad (Crítico, Urgente, Moderado, Bajo), sitio, fecha de creación etc.

HU-02 — Asignar técnico a incidente

Como administrador, quiero asignar un técnico a un incidente registrado, 
para que pueda comenzar a atenderlo.

Criterios:

El estado cambia de Registrado a Asignado
Solo se puede asignar un técnico con especialidad coincidente
El técnico no puede tener más de 3 incidentes activos

HU-03 — Avanzar estado del incidente

Como técnico, quiero cambiar el estado de mi incidente asignado,
para reflejar el progreso real del trabajo.

Criterios:

Solo puede avanzar en orden: Asignado → En progreso → Resuelto → Cerrado
No se puede saltar estados ni retroceder.

HU-04 — Reasignar incidente

Como administrador, quiero reasignar un incidente a otro técnico, para balancear la carga de trabajo.

Criterios:

El técnico anterior queda liberado
El nuevo técnico debe cumplir las mismas reglas especialidad + límite de 3
Se registra en el historial


HU-05 — Escalado automático

Como sistema, quiero marcar automáticamente como "Escalado" los incidentes Críticos o Urgentes que lleven más de 2 horas en estado Registrado,
para que no queden sin atención.

Criterios:

Solo aplica a severidad Crítico o Urgente
Si llevan +2 horas en Registrado sin cambio, se marca Escalado
Se registra en el historial

HU-06 — Ver historial de cambios de un incidente

Como administrador, quiero ver el historial completo de cambios de estado de un incidente,
para tener trazabilidad total.

Criterios:

Muestra: estado anterior, estado nuevo, fecha, técnico responsable
Ordenado cronológicamente

HU-07 — Listar incidentes activos de un técnico

Como técnico, quiero ver mis incidentes activos, para saber cuántos tengo y cuáles son.

Criterios:

Muestra incidentes con estado distinto a Cerrado
Indica cuántos activos tiene (máximo 3)

HU-08 — Registrar técnico

Como administrador, quiero registrar un técnico con su especialidad, para que pueda ser asignado a incidentes.

Criterios:

Especialidades: fibra óptica, microondas, sistemas eléctricos
El técnico queda disponible para asignaciones.

HU-09 — Registrar sitio de red

Como administrador, quiero registrar los sitios de red (antenas, nodos, POP), 
para poder asociarlos a incidentes.

Criterios:

Campos: nombre, tipo, ubicación
Debe existir antes de crear un incidente

HU-10 — Generar reporte de incidentes

Como administrador, quiero generar un reporte de incidentes filtrando por estado o técnico, 
para cumplimiento de SLA.

Criterios:

Filtros: por estado, por técnico, por rango de fechas
Devuelve lista con datos del incidente


HU-11 — Ver incidentes escalados

Como administrador, quiero ver todos los incidentes escalados, para atenderlos con prioridad.

Criterios:

Lista solo incidentes con estado Escalado
Muestra tiempo que llevan escalados
