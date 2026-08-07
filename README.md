# EquipIT — Guide du mentor

> **Version corrigée de référence** pour le projet OpenClassrooms « EquipIT : gestion du matériel informatique en prêt ».

Ce dépôt contient la **solution corrigée et commentée** de l'exercice. Il sert de guide officiel aux mentors OpenClassrooms pour corriger les copies des étudiants, lancer l'application et vérifier chaque exigence du sujet.

---

## 1. Présentation du projet

**EquipIT** est une application de gestion de prêt de matériel informatique (écrans, claviers, souris…) pour les employés d'une entreprise.

Elle est volontairement découplée en **deux projets** + un projet de tests :

| Projet | Rôle | Technologie |
|---|---|---|
| `EquipIT.Api` | API REST (données métier, règles d'emprunt) | ASP.NET Core Web API (.NET 10) |
| `EquipIT.Front` | Interface utilisateur | Blazor WebAssembly (standalone) |
| `EquipIT.Tests` | Tests unitaires du service métier | xUnit |

Fonctionnalités clés :

- Lister le matériel disponible et emprunté.
- Emprunter un équipement en renseignant le **prénom** et le **nom** de l'emprunteur (la date d'emprunt est enregistrée automatiquement).
- Retourner un équipement (la date de retour est enregistrée automatiquement).
- Interface **bilingue** (français par défaut / anglais), via un sélecteur de langue.
- Données démarrées automatiquement (base SQLite + migrations EF Core).

---

## 2. Comment lancer le projet

### Prérequis

- SDK **.NET 10** (ou plus récent)
- Un navigateur récent (Chrome, Edge, Firefox…)

### 1. Restauration des dépendances

```bash
dotnet restore
```

### 2. Lancer l'API (`EquipIT.Api`)

```bash
dotnet run --project EquipIT.Api
```

L'API démarre sur :

- HTTPS : <https://localhost:7093>
- HTTP : <http://localhost:5224>
- Swagger / OpenAPI : <https://localhost:7093/openapi/v1.json>

> ⚠️ Le **port HTTPS est important** : le front Blazor est configuré pour appeler `https://localhost:7093`. Si l'API est lancée sur un autre port, mettre à jour l'adresse dans `EquipIT.Front/Program.cs`.

### 3. Lancer le frontend (`EquipIT.Front`)

Dans un **second terminal** :

```bash
dotnet run --project EquipIT.Front
```

Le front Blazor WebAssembly démarre sur :

- HTTPS : <https://localhost:7211>
- HTTP : <http://localhost:5200>

> Ouvrir de préférence l'URL **HTTPS** (`https://localhost:7211`), car l'API est appelée en HTTPS (évite les erreurs de *mixed content*).

### 4. Lancer les tests

```bash
dotnet test EquipIT.Tests
```

---

## 3. Architecture attendue

Le sujet demande une architecture **simple mais correctement découplée**, adaptée à des débutants.

### Séparation des responsabilités

- **`Models/`** — les entités du domaine (`Equipment`) et les DTO (`BorrowRequest`).
- **`Data/`** — le `DbContext` EF Core et la base SQLite.
- **`Services/`** — la logique métier (`IEquipmentService` / `EquipmentService`).
- **`Controllers/`** — les points d'entrée REST, qui délèguent au service.

### Interface et injection de dépendances

- La logique métier est exposée via une **interface** (`IEquipmentService`), implémentée par `EquipmentService`.
- L'interface et le service sont enregistrés dans le conteneur DI (`Program.cs`) en **Scoped**.
- Le contrôleur et le service reçoivent leurs dépendances **par constructeur** (jamais de `new`).
- Les tests utilisent une **base InMemory EF Core** pour vérifier la logique sans toucher au vrai fichier SQLite.

### API REST attendue (Exercice 1 & 2)

| Méthode | Route | Description | Réponses |
|---|---|---|---|
| `GET` | `/api/equipment` | Liste tout le matériel | `200 OK` |
| `PUT` | `/api/equipment/{id}/borrow` | Emprunte le matériel (corps JSON : `firstName`, `lastName`) | `204 No Content` / `400 Bad Request` |
| `PUT` | `/api/equipment/{id}/return` | Retourne le matériel | `204 No Content` / `400 Bad Request` |

Règles métier (dans `EquipmentService`) :

- Emprunter un équipement **déjà emprunté** → exception → `400 Bad Request`.
- Retourner un équipement **disponible** → exception → `400 Bad Request`.
- L'emprunt enregistre `BorrowDate` (UTC) ; le retour enregistre `LastReturnDate` (UTC) et libère l'équipement.

---

## 4. Précision sur la traduction (i18n)

**Point important pour la correction : ne pas pénaliser les étudiants sur les données !**

- L'**interface** Blazor (boutons, en-têtes de tableau, statuts, placeholders…) est traduite en français et en anglais via des fichiers de ressources **`.resx`** (`App.resx` pour le français par défaut, `App.en-US.resx` pour l'anglais).
- Les **données de la base** (noms d'équipements comme « Keyboard », catégories comme « Input ») **restent en anglais** : c'est un choix voulu du sujet, le front affiche simplement ce que renvoie l'API.
- Le passage d'une langue à l'autre se fait via un sélecteur **FR / EN** dans l'en-tête. Le choix est mémorisé (`localStorage`) et appliqué au redémarrage.

> ✅ **Attendu chez l'étudiant** : un sélecteur de langue fonctionnel qui change la langue **sans crash**. Voir la grille de vérification ci-dessous.

---

## 5. Grille de vérification (Checklist)

### Architecture & code

- [ ] Le projet est découplé en `Api`, `Front` (Blazor WASM) et `Tests`.
- [ ] La logique métier est derrière une **interface** (`IEquipmentService`).
- [ ] **Injection de dépendances** par constructeur (pas de `new` dans les contrôleurs).
- [ ] Le `DbContext` est enregistré dans `Program.cs` et la base est créée/migrée au démarrage.
- [ ] Code propre, simple et lisible (adapté à des débutants, commentaires pertinents).

### API REST

- [ ] `GET /api/equipment` renvoie la liste (`200 OK`).
- [ ] `PUT /api/equipment/{id}/borrow` accepte `firstName` / `lastName` et renvoie `204` (ou `400` si déjà emprunté).
- [ ] `PUT /api/equipment/{id}/return` renvoie `204` (ou `400` si déjà disponible).
- [ ] Les dates d'emprunt / de retour sont bien enregistrées.
- [ ] Swagger/OpenAPI actif.

### Frontend Blazor

- [ ] Le front appelle bien l'API (CORS configuré, bonne adresse de base).
- [ ] Le tableau affiche : Nom, Catégorie, Statut, Emprunteur, Date d'emprunt, Dernier retour, Action.
- [ ] Le bouton « Emprunter » ouvre un formulaire (prénom / nom) avant confirmation.
- [ ] Le bouton « Retourner » rend l'équipement disponible.
- [ ] Le titre de l'application est « EquipIT » (menu **et** onglet du navigateur).

### i18n (traduction)

- [ ] Le français (`fr-FR`) est la langue par défaut.
- [ ] Le sélecteur FR / EN change la langue de l'interface.
- [ ] Le changement de langue est mémorisé (rechargement sans perte de la langue choisie).
- [ ] **Aucun crash** au changement de langue ni au rechargement (« Unhandled error » interdit).
- [ ] Les `.resx` contiennent au moins : nom de l'app, en-têtes, statuts, boutons d'action.
- [ ] ⚠️ Ne **pas** exiger la traduction des données de la base (noms/catégories restent en anglais).

### Tests (si présents)

- [ ] Des tests xUnit couvrent l'emprunt et le retour (cas nominaux + cas d'erreur).
- [ ] `dotnet test` passe.

---

*Ce document fait partie de la solution corrigée de référence. Il est destiné aux mentors OpenClassrooms.*
