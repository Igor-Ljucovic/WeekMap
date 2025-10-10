import { BrowserRouter, Route, Routes } from "react-router-dom";
import Layout from "./Components/Layout/Layout";
import HomePage from "./Components/Pages/HomePage";
import RegisterPage from "./Components/Pages/RegisterPage";
import LoginPage from "./Components/Pages/LoginPage";
import SettingsPage from "./Components/Pages/SettingsPage";
import ActivityTemplatesPage from "./Components/Pages/ActivityTemplatesPage";
import ActivityCategoriesPage from "./Components/Pages/ActivityCategoriesPage";
import WeekMapsPage from "./Components/Pages/WeekMapsPage";

function App() {
    return (
        <BrowserRouter>
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
        </BrowserRouter>
    );
}

export default App;
