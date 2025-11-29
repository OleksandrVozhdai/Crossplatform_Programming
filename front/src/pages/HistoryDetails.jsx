import React, { useEffect, useState } from "react";
import Header from "../components/Header";
import { useLocation, useParams } from "react-router-dom";

export default function HistoryDetails() {
    const { country: paramsCountry } = useParams();
    const location = useLocation();

    // дані, які могли бути передані через navigate
    const passedCountry = location.state?.country;
    const passedRecord = location.state?.record;

    const [country, setCountry] = useState(paramsCountry || passedCountry || "");
    const [record, setRecord] = useState(passedRecord || null);
    const [error, setError] = useState(null);
    const [daysData, setDaysData] = useState(null);

    useEffect(() => {
        // Якщо дані вже передані із History.js — не фетчимо повторно
        if (record) return;

        if (!country) {
            setError("No country name provided.");
            return;
        }

        const fetchByName = async () => {
            try {
                const res = await fetch(`https://localhost:7147/api/v2/CaseRecord/Name/${country}`);
                if (!res.ok) throw new Error("Country not found");

                const data = await res.json();
                setRecord(data);

            } catch (err) {
                console.error(err);
                setError("Failed to load country data.");
            }
        };

        fetchByName();
    }, [country]);

    return (
        <>
            <Header />

            <div className="container">
                <h1>COVID-19 Data for {country}</h1>

                {error ? (
                    <div className="alert alert-warning">{error}</div>
                ) : record ? (
                    <>
                        <div className="stats-grid">
                            <div className="stat-card">
                                <h3>Total Cases</h3>
                                <p>{record.cases?.toLocaleString() ?? "N/A"}</p>
                            </div>

                            <div className="stat-card">
                                <h3>Today Cases</h3>
                                <p>{record.todayCases?.toLocaleString() ?? "N/A"}</p>
                            </div>

                            <div className="stat-card">
                                <h3>Deaths</h3>
                                <p>{record.deaths?.toLocaleString() ?? "N/A"}</p>
                            </div>

                            <div className="stat-card">
                                <h3>Today Deaths</h3>
                                <p>{record.todayDeaths?.toLocaleString() ?? "N/A"}</p>
                            </div>

                            <div className="stat-card">
                                <h3>Recovered</h3>
                                <p>{record.recovered?.toLocaleString() ?? "N/A"}</p>
                            </div>

                            <div className="stat-card">
                                <h3>Today Recovered</h3>
                                <p>{record.todayRecovered?.toLocaleString() ?? "N/A"}</p>
                            </div>

                            <div className="stat-card">
                                <h3>Population</h3>
                                <p>{record.population?.toLocaleString() ?? "N/A"}</p>
                            </div>

                            <div className="stat-card">
                                <h3>Updated</h3>
                                <p>{new Date(record.updatedAt).toLocaleDateString()}</p>
                            </div>
                        </div>

                        {daysData && (
                            <div className="stats-grid">
                                <div className="stat-card">
                                    <h3>Deaths Change (last {daysData.days} days)</h3>
                                    <p>{daysData.death}</p>
                                </div>

                                <div className="stat-card">
                                    <h3>Recovered Change (last {daysData.days} days)</h3>
                                    <p>{daysData.recovered}</p>
                                </div>
                            </div>
                        )}
                    </>
                ) : (
                    <div className="alert alert-info">
                        Loading data… If error, check console.
                    </div>
                )}
            </div>

            <div className="worldmap-container"></div>
        </>
    );
}
