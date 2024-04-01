import AddMoviesForm from "../Components/Admin/AddMoviesForm";
import MainLayout from "../Components/Layout/MainLayout";

import axios from "axios";

const AddMovies = () => {

    return (
        <MainLayout>
            <AddMoviesForm />
        </MainLayout>
    )
}

export default AddMovies;