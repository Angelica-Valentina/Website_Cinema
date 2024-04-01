import Header from '../Header/Header';
import SideBar from '../Side_Bar/SideBar';
import './MainLayout.css';

function MainLayout({ children }) 
{
    return (
        <div>
            <Header/>
            <SideBar/>

            <div className='main-layout'>
                {children}
            </div>
        </div>
    );
}

export default MainLayout;

