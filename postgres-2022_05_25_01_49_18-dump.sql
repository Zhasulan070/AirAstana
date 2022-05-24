--
-- PostgreSQL database dump
--

-- Dumped from database version 13.1
-- Dumped by pg_dump version 13.1

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- Name: main; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA main;


ALTER SCHEMA main OWNER TO postgres;

--
-- Name: _flight_enum_status; Type: TYPE; Schema: main; Owner: postgres
--

CREATE TYPE main._flight_enum_status (
    INTERNALLENGTH = variable,
    INPUT = array_in,
    OUTPUT = array_out,
    RECEIVE = array_recv,
    SEND = array_send,
    ANALYZE = array_typanalyze,
    ELEMENT = ???,
    CATEGORY = 'A',
    ALIGNMENT = int4,
    STORAGE = extended
);


ALTER TYPE main._flight_enum_status OWNER TO postgres;

--
-- Name: _flight_status_enum; Type: TYPE; Schema: public; Owner: postgres
--

CREATE TYPE public._flight_status_enum (
    INTERNALLENGTH = variable,
    INPUT = array_in,
    OUTPUT = array_out,
    RECEIVE = array_recv,
    SEND = array_send,
    ANALYZE = array_typanalyze,
    ELEMENT = ???,
    CATEGORY = 'A',
    ALIGNMENT = int4,
    STORAGE = extended
);


ALTER TYPE public._flight_status_enum OWNER TO postgres;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: flight; Type: TABLE; Schema: main; Owner: postgres
--

CREATE TABLE main.flight (
    id uuid NOT NULL,
    origin character varying(256),
    destination character varying(256),
    departure date,
    arrival date,
    status character varying(256)
);


ALTER TABLE main.flight OWNER TO postgres;

--
-- Name: role; Type: TABLE; Schema: main; Owner: postgres
--

CREATE TABLE main.role (
    id uuid NOT NULL,
    code text NOT NULL
);


ALTER TABLE main.role OWNER TO postgres;

--
-- Name: user; Type: TABLE; Schema: main; Owner: postgres
--

CREATE TABLE main."user" (
    id uuid NOT NULL,
    username character varying(256),
    password character varying(256),
    role_id uuid
);


ALTER TABLE main."user" OWNER TO postgres;

--
-- Data for Name: flight; Type: TABLE DATA; Schema: main; Owner: postgres
--

COPY main.flight (id, origin, destination, departure, arrival, status) FROM stdin;
b36933ab-2931-46f9-8e85-41e73c89aeb9	a	b	2022-05-24	2022-05-24	Cancelled
\.


--
-- Data for Name: role; Type: TABLE DATA; Schema: main; Owner: postgres
--

COPY main.role (id, code) FROM stdin;
f62edfef-8cc2-48fa-9d94-a7f0cf1165ae	client
60d6d1ea-b908-4700-b959-6bed2ac15cde	Moderator
\.


--
-- Data for Name: user; Type: TABLE DATA; Schema: main; Owner: postgres
--

COPY main."user" (id, username, password, role_id) FROM stdin;
df5b9a54-944e-481d-95bd-80fb20cfd6e0	client	IO1yrMAGFw4=	f62edfef-8cc2-48fa-9d94-a7f0cf1165ae
f2adda66-bb3f-4fb9-a424-16666389f6b4	admin	y0pOy6EEgrc=	60d6d1ea-b908-4700-b959-6bed2ac15cde
\.


--
-- Name: flight flight_pk; Type: CONSTRAINT; Schema: main; Owner: postgres
--

ALTER TABLE ONLY main.flight
    ADD CONSTRAINT flight_pk PRIMARY KEY (id);


--
-- Name: role role_pk; Type: CONSTRAINT; Schema: main; Owner: postgres
--

ALTER TABLE ONLY main.role
    ADD CONSTRAINT role_pk PRIMARY KEY (id);


--
-- Name: user user_pk; Type: CONSTRAINT; Schema: main; Owner: postgres
--

ALTER TABLE ONLY main."user"
    ADD CONSTRAINT user_pk PRIMARY KEY (id);


--
-- Name: role_code_uindex; Type: INDEX; Schema: main; Owner: postgres
--

CREATE UNIQUE INDEX role_code_uindex ON main.role USING btree (code);


--
-- Name: role_id_uindex; Type: INDEX; Schema: main; Owner: postgres
--

CREATE UNIQUE INDEX role_id_uindex ON main.role USING btree (id);


--
-- Name: user_id_uindex; Type: INDEX; Schema: main; Owner: postgres
--

CREATE UNIQUE INDEX user_id_uindex ON main."user" USING btree (id);


--
-- Name: user_username_uindex; Type: INDEX; Schema: main; Owner: postgres
--

CREATE UNIQUE INDEX user_username_uindex ON main."user" USING btree (username);


--
-- Name: user user_role_id_fk; Type: FK CONSTRAINT; Schema: main; Owner: postgres
--

ALTER TABLE ONLY main."user"
    ADD CONSTRAINT user_role_id_fk FOREIGN KEY (role_id) REFERENCES main.role(id) ON UPDATE CASCADE ON DELETE SET NULL;


--
-- PostgreSQL database dump complete
--

