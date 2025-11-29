import React, { useEffect, useState } from "react";
import Header from "../components/Header";

export default function Central() {

    const [caseRecords, setCaseRecords] = useState([]);

    useEffect(() => {
        loadCountries();
    }, []);

    async function loadCountries() {
        try {
            const res = await fetch("https://localhost:7147/api/v2/CaseRecord");
            const data = await res.json();

            setCaseRecords(data);
        } catch (err) {
            console.error("Error loading country data:", err);
        }
    }

    return (
        <>

            <Header/>

            {/* TITLE */}
            <h2 style={{ color: "#569cd6", textAlign: "center", marginTop: "20px" }}>
                Case Records Table (All Countries)
            </h2>

            {/* CASE RECORDS TABLE */}
            <table>
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Country</th>
                        <th>Cases</th>
                        <th>Today Cases</th>
                        <th>Deaths</th>
                        <th>Today Deaths</th>
                        <th>Recovered</th>
                        <th>Today Recovered</th>
                        <th>Population</th>
                        <th>Active</th>
                        <th>Critical</th>
                        <th>Latitude</th>
                        <th>Longitude</th>
                        <th>Updated</th>
                    </tr>
                </thead>

                <tbody>
                    {caseRecords.length > 0 ? (
                        caseRecords.map((item) => (
                            <tr key={item.id}>
                                <td>{item.id}</td>
                                <td>{item.country}</td>
                                <td>{item.cases.toLocaleString()}</td>
                                <td>{item.todayCases.toLocaleString()}</td>
                                <td>{item.deaths.toLocaleString()}</td>
                                <td>{item.todayDeaths.toLocaleString()}</td>
                                <td>{item.recovered.toLocaleString()}</td>
                                <td>{item.todayRecovered.toLocaleString()}</td>
                                <td>{item.population.toLocaleString()}</td>
                                <td>{item.active.toLocaleString()}</td>
                                <td>{item.critical.toLocaleString()}</td>
                                <td>{item.latitude}</td>
                                <td>{item.longitude}</td>
                                <td>{item.updatedAt?.split("T")[0]}</td>
                            </tr>
                        ))
                    ) : (
                        <tr>
                            <td colSpan="14" style={{ textAlign: "center" }}>
                                Loading country data...
                            </td>
                        </tr>
                    )}
                </tbody>
            </table>
        </>
    );
}
