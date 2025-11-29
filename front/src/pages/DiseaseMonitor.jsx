import React, { useState } from "react";
import Header from "../components/Header";

export default function DiseaseMonitor() {
    const [country, setCountry] = useState("");
    const [days, setDays] = useState("");
    const [record, setRecord] = useState(null);
    const [error, setError] = useState("");

    const fetchData = async () => {
        try {
            setError("");
            setRecord(null);

            const url = `https://localhost:7147/api/v2/CaseRecord/Name/${country}`;
            const resp = await fetch(url);

            if (!resp.ok) {
                setError("Country not found");
                return;
            }

            const data = await resp.json();
            setRecord(data);

        } catch (e) {
            console.error(e);
            setError("Error loading data. Check server connection.");
        }
    };

    const handleSubmit = e => {
        e.preventDefault();
        fetchData();
    };

    const format = (n) => (n > 0 ? n.toLocaleString() : "N/A");

    return (
        <>
            <Header />

            <div className="container">

                {/* Title */}
                <h1>COVID-19 Data for {country || "..."}</h1>

                {/* Errors */}
                {error && <div className="alert alert-warning">{error}</div>}

                {/* Data grid */}
                {record ? (
                    <>
                        <div className="stats-grid">
                            <div className="stat-card">
                                <h3>Total Cases</h3>
                                <p>{format(record.cases)}</p>
                            </div>

                            <div className="stat-card">
                                <h3>Today Cases</h3>
                                <p>{format(record.todayCases)}</p>
                            </div>

                            <div className="stat-card">
                                <h3>Deaths</h3>
                                <p>{format(record.deaths)}</p>
                            </div>

                            <div className="stat-card">
                                <h3>Today Deaths</h3>
                                <p>{format(record.todayDeaths)}</p>
                            </div>

                            <div className="stat-card">
                                <h3>Recovered</h3>
                                <p>{format(record.recovered)}</p>
                            </div>

                            <div className="stat-card">
                                <h3>Today Recovered</h3>
                                <p>{format(record.todayRecovered)}</p>
                            </div>

                            <div className="stat-card">
                                <h3>Population</h3>
                                <p>{format(record.population)}</p>
                            </div>

                            <div className="stat-card">
                                <h3>Updated</h3>
                                <p>{new Date(record.updatedAt).toLocaleDateString()}</p>
                            </div>
                        </div>
                    </>
                ) : (
                    <div className="alert alert-info">Enter a country and fetch data.</div>
                )}

                {/* Form */}
                <form className="filter-form" onSubmit={handleSubmit}>
                    <label>Country:</label>
                    <input
                        type="text"
                        placeholder="e.g. Ukraine"
                        value={country}
                        onChange={e => setCountry(e.target.value)}
                    />

                    <button type="submit">Fetch Data</button>
                </form>

                <div className="worldmap-container"></div>
            </div>
        </>
    );
}
