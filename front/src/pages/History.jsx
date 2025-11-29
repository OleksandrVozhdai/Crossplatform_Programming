import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import Header from "../components/Header";

export default function History() {
    const [filter, setFilter] = useState("");
    const [countries, setCountries] = useState([]);
    const navigate = useNavigate();

    // Load countries from API
    useEffect(() => {
        const fetchCountries = async () => {
            try {
                const resp = await fetch("https://localhost:7147/api/v2/CaseRecord");
                const data = await resp.json();
                setCountries(data);
            } catch (e) {
                console.error("Failed to load countries:", e);
            }
        };

        fetchCountries();
    }, []);

    // Filtering
    const filtered = countries.filter(c =>
        c.country.toLowerCase().includes(filter.toLowerCase())
    );

    const handleDetails = (countryName) => {
        navigate("/history/details", { state: { country: countryName } });
    };

    return (
        <>
            <Header />

            <h1 className="page-title">History</h1>

            <form
                style={{
                    width: "80%",
                    display: "flex",
                    justifyContent: "center",
                    margin: "30px auto"
                }}
                onSubmit={(e) => e.preventDefault()}
            >
                <label style={{ marginRight: "10px" }}>Search by country:</label>
                <input
                    type="text"
                    placeholder="e.g., Ukraine"
                    value={filter}
                    onChange={(e) => setFilter(e.target.value)}
                />
                <button type="submit">Search</button>
            </form>

            <div className="stats-grid-countries">
                {filtered.map((c) => (
                    <div key={c.id} className="stat-card country-card">
                        <h3>{c.country}</h3>

                        <button
                            onClick={() =>
                                navigate("/HistoryDetails/" + c.country, {
                                    state: {
                                        country: c.country,
                                        record: c
                                    }
                                })
                            }
                        >
                            View Details
                        </button>
                    </div>
                ))}
            </div>
        </>
    );
}
