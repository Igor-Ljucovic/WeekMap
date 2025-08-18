import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import Layout from "./Components/Layout";
import HomePage from "./Components/HomePage";
import RegisterPage from "./Components/RegisterPage";
import LoginPage from "./Components/LoginPage";
import SettingsPage from "./Components/SettingsPage";
import ActivityTemplatesPage from "./Components/ActivityTemplatesPage";
import ActivityCategoriesPage from "./Components/ActivityCategoriesPage";
import WeekMapsPage from "./Components/WeekMapsPage";

function App() {
    return (
        <Router>
            <Routes>
                <Route path="/" element={<Layout />}>
                    <Route path="" element={<HomePage />} />
                    <Route path="/settings" element={<SettingsPage />} />
                    <Route path="/activity-templates" element={<ActivityTemplatesPage />} />
                    <Route path="/activity-categories" element={<ActivityCategoriesPage />} />
                    <Route path="/weekmaps" element={<WeekMapsPage />} />
                </Route>
                
                <Route path="/register" element={<RegisterPage />} />
                <Route path="/login" element={<LoginPage />} />
            </Routes>
        </Router>
    );
}

export default App;
