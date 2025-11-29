import React, { useState } from "react";
import Header from "../components/Header";

export default function CompareCountries() {
  const [country1, setCountry1] = useState("");
  const [country2, setCountry2] = useState("");

  const [record1, setRecord1] = useState(null);
  const [record2, setRecord2] = useState(null);
  const [error, setError] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      setError("");
      setRecord1(null);
      setRecord2(null);

      const url1 = `https://localhost:7147/api/v2/CaseRecord/Name/${country1}`;
      const url2 = `https://localhost:7147/api/v2/CaseRecord/Name/${country2}`;

      const [res1, res2] = await Promise.all([fetch(url1), fetch(url2)]);

      if (!res1.ok || !res2.ok) {
        setError("One or both countries were not found");
        return;
      }

      const data1 = await res1.json();
      const data2 = await res2.json();

      setRecord1(data1);
      setRecord2(data2);
    } catch (err) {
      console.log(err);
      setError("Server error");
    }
  };

  const formatNumber = (num) =>
    num > 0 ? num.toLocaleString("en-US") : "N/A";

  return (
    <>
      <Header />

      <div className="container">
        <h1>Compare COVID-19 Data</h1>

        <form onSubmit={handleSubmit}>
          <input
            type="text"
            value={country1}
            onChange={(e) => setCountry1(e.target.value)}
            placeholder="Country 1"
            required
          />
          <input
            type="text"
            value={country2}
            onChange={(e) => setCountry2(e.target.value)}
            placeholder="Country 2"
            required
          />
          <button type="submit">Compare</button>
        </form>

        {error && <div className="alert alert-warning">{error}</div>}

        {!error && record1 && record2 && (
          <>
            {/* === COUNTRY 1 === */}
            <h1 style={{ marginBottom: "10px", marginTop: "20px", fontSize: "25px" }}>
              {record1.country}
            </h1>
            <div className="stats-grid">
              <div className="stat-card">
                <h3>Total Cases</h3>
                <p>{formatNumber(record1.cases)}</p>
              </div>
              <div className="stat-card">
                <h3>Deaths</h3>
                <p>{formatNumber(record1.deaths)}</p>
              </div>
              <div className="stat-card">
                <h3>Recovered</h3>
                <p>{formatNumber(record1.recovered)}</p>
              </div>
              <div className="stat-card">
                <h3>Population</h3>
                <p>{formatNumber(record1.population)}</p>
              </div>
            </div>

            {/* === COUNTRY 2 === */}
            <h1 style={{ marginBottom: "10px", fontSize: "25px" }}>
              {record2.country}
            </h1>
            <div className="stats-grid">
              <div className="stat-card">
                <h3>Total Cases</h3>
                <p>{formatNumber(record2.cases)}</p>
              </div>
              <div className="stat-card">
                <h3>Deaths</h3>
                <p>{formatNumber(record2.deaths)}</p>
              </div>
              <div className="stat-card">
                <h3>Recovered</h3>
                <p>{formatNumber(record2.recovered)}</p>
              </div>
              <div className="stat-card">
                <h3>Population</h3>
                <p>{formatNumber(record2.population)}</p>
              </div>
            </div>
          </>
        )}
      </div>
    </>
  );
}
