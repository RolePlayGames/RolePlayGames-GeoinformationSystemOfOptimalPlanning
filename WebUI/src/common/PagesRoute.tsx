import { Route } from "react-router-dom";
import { Fragment } from "react/jsx-runtime";

type PagesRouteProps = {
    path: string,
    element: JSX.Element,
}

export const PagesRoute = ({ path, element }: PagesRouteProps) => (
    <Fragment>        
        <Route path={path} element={element}/>
        <Route path={`${path}/:id`} element={element}/>
    </Fragment>
);