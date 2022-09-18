# Analyse der Auswirkungen verschiedener Anwendungsarchitekturen auf den CO<sub>2</sub>-Fußabdruck von Cloud-Anwendungen am Beispiel von Microsoft Azure

## Kurzbeschreibung der Aufgabe

Diese Bachelorarbeit untersucht den CO<sub>2</sub>-äquivalenten Fußabdruck von beispielhaften Standard-Cloud-Anwendungen unter Berücksichtigung möglicher Anwendungsarchitekturen. Als Standardanwendungsfall werden die verschiedenen Szenarien eines Web-Shops verwendet.

Die betrachteten Komponenten der Webanwendung sind: Bestellverarbeitung als Benutzerinteraktion, Rechnungsstellung und Versandbestätigung als Hintergrundprozesse sowie die Benutzerverwaltung über OpenID Connect als eine externe SaaS-Lösung. Zur Beurteilung wird eine standardisierte Last mit drei Stufen auf die Anwendung gebracht.

Als Hosting-Umgebung werden VM-basierte Systeme, containerbasierte Systeme, Platform-as-a-Service (PaaS) Dienste wie Azure Functions und Azure App-Services sowie Software-as-a-Service (SaaS) Dienste wie ADD B2C (Identity Management) betrachtet. Abhängige Dienste wie Storage, Datenbanken und Messaging werden in die Untersuchung mit aufgenommen.

Microsoft Azure bietet ein Monitoring, um den CO<sub>2</sub>-Fußabdruck zu bewerten. Basierend auf diesen Informationen sowie weiter zu untersuchenden Kennzahlen, wird die Auswirkung einer Anwendungsarchitektur begutachtet. Hierbei stehen dann die CO<sub>2</sub>-äquivalenten Emissionen der Performance und Benutzbarkeit (Usability) gegenüber.

Der Beispielanwendungsfall wird mehrfach unter Verwendung der verschiedenen Anwendungsarchitekturen umgesetzt. Hierbei wird jeweils versucht anhand des Azure Klima-Monitorings die Emissionen zu minimieren und dabei die Performance und Benutzbarkeit (Usability) zu bewerten. Bei der Bewertung wird versucht, nicht nur den direkten CO<sub>2</sub>-Fußabdruck durch den Energieverbrauch zu betrachten, sondern den gesamten Ressourcenverbrauch der Anwendung.