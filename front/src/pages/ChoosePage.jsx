import React from "react";
import Header from "../components/Header";
import corona from "../assets/corona.png";
import { useNavigate } from "react-router-dom";



export default function ChoosePage() {

        const navigate = useNavigate();

  return (
    <>
        <Header/>

      <div className="center-buttons">
        <button
          onClick={() => navigate("/monitor")}
          className="white-button"
          style={{ marginTop: "-100px" }}
        >
          Fetch Country Data
        </button>

        <button
          onClick={() => navigate("/compare")}
          className="white-button"
          style={{ marginTop: 20 }}
        >
          Compare Countries
        </button>

        <button
          onClick={() => navigate("/history")}
                    className="white-button"
          style={{ marginTop: "-220px" }}
        >
          History
        </button>

        <button
          onClick={() => (window.location.href = "/central")}
          className="white-button"
          style={{ marginTop: 140 }}
        >
          Central Table
        </button>

        <img alt="" src={corona} />
      </div>
    </>
  );
}
