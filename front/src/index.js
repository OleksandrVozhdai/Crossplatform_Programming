import React from 'react';
import ReactDOM from 'react-dom/client';
import { BrowserRouter, Routes, Route } from 'react-router-dom';

import HomePage from './pages/HomePage';
import ChoosePage from './pages/ChoosePage';
import CompareCountries from './pages/CompareCountries';
import DiseaseMonitor from './pages/DiseaseMonitor';
import History from './pages/History';
import HistoryDetails from './pages/HistoryDetails';
import Central from './pages/Central';

const root = ReactDOM.createRoot(document.getElementById('root'));

root.render(
  <React.StrictMode>
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<HomePage />} />
        <Route path="/choose" element={<ChoosePage />} />
        <Route path="/compare" element={<CompareCountries />} />
        <Route path="/monitor" element={<DiseaseMonitor />} />
        <Route path="/history" element={<History />} />
                <Route path="/HistoryDetails/:country" element={<HistoryDetails />} />
                        <Route path="/central" element={<Central />} />
      </Routes>
    </BrowserRouter>
  </React.StrictMode>
);
